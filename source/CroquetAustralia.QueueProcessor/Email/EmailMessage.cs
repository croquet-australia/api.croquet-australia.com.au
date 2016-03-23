using System;
using System.Collections.Generic;
using System.Linq;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using MarkdownSharp;
using NodaTime;
using NodaTime.Text;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailMessage
    {
        private readonly string _baseUrl;
        private readonly Lazy<string> _bodyAsHtml;
        private readonly Lazy<string> _bodyAsText;
        private readonly EntrySubmitted _event;
        private readonly SubmitEntry.PlayerClass _sendTo;
        private readonly string _template;
        private readonly Tournament _tournament;
        private readonly TournamentItem _tournamentEvent;

        public IEnumerable<EmailAddress> Bcc;

        public EmailMessage(EmailMessageSettings emailMessageSettings, Tournament tournament, string template, EntrySubmitted @event, SubmitEntry.PlayerClass sendTo)
        {
            _tournament = tournament;
            _template = template;
            _event = @event;
            _sendTo = sendTo;

            _bodyAsHtml = new Lazy<string>(CreateBodyAsHtml);
            _bodyAsText = new Lazy<string>(CreateBodyAsText);
            _tournamentEvent = _event.EventId.HasValue ? _tournament.Events.Single(e => e.Id == _event.EventId) : null;
            _baseUrl = emailMessageSettings.BaseUrl;

            Bcc = emailMessageSettings.Bcc;
        }

        public IEnumerable<EmailAddress> To => new[] {new EmailAddress(_sendTo.Email, $"{_sendTo.FirstName} {_sendTo.LastName}")};

        // todo: remove hard coding
        public EmailAddress From => new EmailAddress("events@croquet-australia.com.au", "Croquet Australia - Events Committee");

        public string Subject => $"{_tournament.Title} Entry";

        public string BodyAsText => _bodyAsText.Value;

        public string BodyAsHtml => _bodyAsHtml.Value;

        private string CreateBodyAsHtml()
        {
            var parser = new Markdown(new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewlines = true,
                LinkEmails = false,
                QuoteSingleLine = true
            });

            const string replacePartnerUrl = "--replace--partner--url--";

            var text = BodyAsText
                .Replace("events@croquet-australia.com.au", "<a href=\"mailto:events@croquet-australia.com.au\">events@croquet-australia.com.au</a>")
                .Replace($"clicking {GetDeposited()}", $"<a href=\"{GetDeposited()}\">clicking here</a>")
                .Replace($"{GetPartnerUrl()}", replacePartnerUrl);

            var html = parser.Transform(text);

            html = html.Replace(replacePartnerUrl, $@"<a href=""{GetPartnerUrl()}"">Croquet Australia website</a>");

            return html;
        }

        private string CreateBodyAsText()
        {
            var formatted = _template
                .Replace("{{Player.FirstName}}", _event.Player.FirstName)
                .Replace("{{Player.LastName}}", _event.Player.LastName)
                .Replace("{{Partner.FirstName}}", _event.Partner?.FirstName)
                .Replace("{{Partner.LastName}}", _event.Partner?.LastName)
                .Replace("{{Partner.EmailAddress}}", _event.Partner?.Email)
                .Replace("{{Tournament.Title}}", _tournament.Title)
                .Replace("{{Tournament.TotalPrice}}", GetTournamentTotalPrice().ToString("C"))
                .Replace("{{Tournament.EventsClose}}", FormatEventsClose(_tournament.EventsClose))
                .Replace("{{Functions}}", GetFunctions())
                .Replace("{{TotalPayable}}", GetTotalPayable().ToString("C"))
                .Replace("{{Stating}}", GetStating())
                .Replace("{{Deposited}}", GetDeposited())
                .Replace("{{PartnerUrl}}", GetPartnerUrl())
                .Replace("{{IndividualPrice}}", _tournamentEvent?.UnitPrice.ToString("C"));

            return formatted;
        }

        private string GetPartnerUrl()
        {
            return string.Format($"{_baseUrl}/tournaments/{_tournament.Starts.Year}/{_tournament.Discipline}/{_tournament.Slug}?FirstName={EncodeQueryStringValue(_event.Partner?.FirstName)}&LastName={EncodeQueryStringValue(_event.Partner?.LastName)}&Email={EncodeQueryStringValue(_event.Partner?.Email)}");
        }

        private static string EncodeQueryStringValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : Uri.EscapeDataString(value);
        }

        private string FormatEventsClose(ZonedDateTime eventsClose)
        {
            var dateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd', 'd' 'MMMM' 'yyyy", DateTimeZoneProviders.Tzdb);
            var result = dateTimePattern.Format(eventsClose);

            return result;
        }

        private string GetDeposited()
        {
            return $"{_baseUrl}/tournaments/deposited?id={_event.EntityId}";
        }

        private string GetStating()
        {
            return $"{_event.Player.FirstName} {_event.Player.LastName} - {_tournament.DepositStating}";
        }

        private decimal GetTotalPayable()
        {
            var functions = _event.Functions.Sum(function => function.TotalPrice);
            var total = functions + GetTournamentTotalPrice();

            return total;
        }

        private string GetFunctions()
        {
            var lineItems = _event.Functions.ToArray();
            var lines = lineItems.Select(lineItem =>
            {
                var function = _tournament.Functions.Single(f => f.Id == lineItem.Id);

                return $"{lineItem.Quantity} * {function.Title} {lineItem.TotalPrice.ToString("C")}";
            });

            return string.Join(Environment.NewLine, lines);
        }

        private decimal GetTournamentTotalPrice()
        {
            if (_tournamentEvent == null)
            {
                return 0;
            }

            var totalPrice = GetIndividualTournamentPrice(_event.Player);

            if (_tournament.IsDoubles && _event.PayingForPartner)
            {
                totalPrice += GetIndividualTournamentPrice(_event.Partner);
            }

            return totalPrice;
        }

        private decimal GetIndividualTournamentPrice(SubmitEntry.PlayerClass player)
        {
            var unitPrice = _tournamentEvent.UnitPrice;
            var applyDiscount = player.FullTimeStudentUnder25 || player.Under21;
            var result = applyDiscount ? unitPrice / 2 : unitPrice;

            return result;
        }
    }
}