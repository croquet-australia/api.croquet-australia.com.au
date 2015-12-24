using System;
using System.Collections.Generic;
using System.Linq;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using MarkdownSharp;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailMessage
    {
        private readonly EntrySubmitted _event;
        private readonly string _template;
        private readonly Tournament _tournament;
        private readonly TournamentItem _tournamentEvent;

        public IEnumerable<EmailAddress> Bcc = new[]
        {
            new EmailAddress("events@croquet-australia.com.au", "Croquet Australia - Events Committee"),
            new EmailAddress("admin@croquet-australia.com.au", "Croquet Australia"),
            new EmailAddress("tim@26tp.com", "Tim Murphy") // todo: review tim
        };

        public EmailMessage(string template, EntrySubmitted @event)
        {
            _template = template;
            _event = @event;

            _tournament = Tournaments.GetById(@event.TournamentId);
            _tournamentEvent = _event.EventId.HasValue ? _tournament.Events.Single(e => e.Id == _event.EventId) : null;
        }

        public IEnumerable<EmailAddress> To => new[] {new EmailAddress(_event.Player.Email, $"{_event.Player.FirstName} {_event.Player.LastName}")};

        // todo: remove hard coding
        public EmailAddress From => new EmailAddress("events@croquet-australia.com.au", "Croquet Australia - Events Committee");

        public string Subject => $"{_tournament.Title} Entry";

        public string BodyAsText()
        {
            var formatted = _template
                .Replace("{{Player.FirstName}}", _event.Player.FirstName)
                .Replace("{{Tournament.Title}}", _tournament.Title)
                .Replace("{{Tournament.TotalPrice}}", GetTournamentTotalPrice().ToString("C"))
                .Replace("{{Functions}}", GetFunctions())
                .Replace("{{TotalPayable}}", GetTotalPayable().ToString("C"))
                .Replace("{{Stating}}", GetStating())
                .Replace("{{Deposited}}", GetDeposited());

            return formatted;
        }

        public string BodyAsHtml()
        {
            var parser = new Markdown(new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewlines = true,
                LinkEmails = false,
                QuoteSingleLine = true
            });

            var text = BodyAsText()
                .Replace("events@croquet-australia.com.au", "<a href=\"mailto:events@croquet-australia.com.au\">events@croquet-australia.com.au</a>")
                .Replace($"clicking {GetDeposited()}", $"<a href=\"{GetDeposited()}\">clicking here</a>");

            var html = parser.Transform(text);

            return html;
        }

        private string GetDeposited()
        {
            return $"https://croquet-australia.com.au/tournaments/deposited?id={_event.EntityId}";
        }

        private string GetStating()
        {
            return $"{_event.Player.FirstName} {_event.Player.LastName} - AC Championship";
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

            var unitPrice = _tournamentEvent.UnitPrice;
            var applyDiscount = _event.Player.FullTimeStudentUnder25 || _event.Player.Under21;

            return applyDiscount ? unitPrice/2 : unitPrice;
        }
    }
}