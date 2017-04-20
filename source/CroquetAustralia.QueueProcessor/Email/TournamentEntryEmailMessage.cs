using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using HeyRed.MarkdownSharp;
using NodaTime;
using NodaTime.Text;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class TournamentEntryEmailMessage
    {
        public static EmailMessage Create(EmailMessageSettings emailMessageSettings, Tournament tournament, string template, EntrySubmitted @event, SubmitEntry.PlayerClass sendTo, IEnumerable<EmailAddress> bcc, EmailAddress from, IEnumerable<FileInfo> attachments = null)
        {
            var tournamentEvent = @event.EventId.HasValue ? tournament.Events.Single(e => e.Id == @event.EventId) : null;

            var to = new[] {new EmailAddress(sendTo.Email, $"{sendTo.FirstName} {sendTo.LastName}")};
            var cc = Enumerable.Empty<EmailAddress>();
            var subject = $"{tournament.Title} Entry";
            var bodyAsText = CreateBodyAsText(template, @event, tournament, tournamentEvent, emailMessageSettings);
            var bodyAsHtml = CreateBodyAsHtml(bodyAsText, emailMessageSettings, tournament, @event);
            var emailAttachments = new EmailAttachments(attachments ?? Enumerable.Empty<FileInfo>());

            return new EmailMessage(to, cc, bcc, from, subject, bodyAsText, bodyAsHtml, emailAttachments);
        }

        private static string CreateBodyAsText(string template, EntrySubmitted entrySubmitted, Tournament tournament, TournamentItem tournamentEvent, EmailMessageSettings emailMessageSettings)
        {
            var formatted = template
                .Replace("{{Player.FirstName}}", entrySubmitted.Player.FirstName)
                .Replace("{{Player.LastName}}", entrySubmitted.Player.LastName)
                .Replace("{{Partner.FirstName}}", entrySubmitted.Partner?.FirstName)
                .Replace("{{Partner.LastName}}", entrySubmitted.Partner?.LastName)
                .Replace("{{Partner.EmailAddress}}", entrySubmitted.Partner?.Email)
                .Replace("{{Tournament.Title}}", tournament.Title)
                .Replace("{{Tournament.TotalPrice}}", GetTournamentTotalPrice(tournamentEvent, entrySubmitted, tournament).ToString("C"))
                .Replace("{{Tournament.EventsClose}}", FormatEventsClose(tournament.EventsClose))
                .Replace("{{Functions}}", GetFunctions(entrySubmitted, tournament))
                .Replace("{{TotalPayable}}", GetTotalPayable(entrySubmitted, tournamentEvent, tournament).ToString("C"))
                .Replace("{{Stating}}", GetStating(entrySubmitted, tournament))
                .Replace("{{Deposited}}", GetDeposited(emailMessageSettings, entrySubmitted))
                .Replace("{{PartnerUrl}}", GetPartnerUrl(emailMessageSettings.BaseUrl, tournament, entrySubmitted))
                .Replace("{{IndividualPrice}}", tournamentEvent?.UnitPrice.ToString("C"))
                .Replace("{{TeamName}}", entrySubmitted.TeamName);

            return formatted;
        }

        private static string CreateBodyAsHtml(string bodyAsText, EmailMessageSettings emailMessageSettings, Tournament tournament, EntrySubmitted entrySubmitted)
        {
            var parser = new Markdown(new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewlines = true,
                LinkEmails = false,
                QuoteSingleLine = true
            });

            const string replacePartnerUrl = "--replace--partner--url--";

            var partnerUrl = GetPartnerUrl(emailMessageSettings.BaseUrl, tournament, entrySubmitted);

            var text = bodyAsText
                .Replace("events@croquet-australia.com.au", "<a href=\"mailto:events@croquet-australia.com.au\">events@croquet-australia.com.au</a>")
                .Replace($"clicking {GetDeposited(emailMessageSettings, entrySubmitted)}", $"<a href=\"{GetDeposited(emailMessageSettings, entrySubmitted)}\">clicking here</a>")
                .Replace($"{partnerUrl}", replacePartnerUrl);

            var html = parser.Transform(text);

            html = html.Replace(replacePartnerUrl, $@"<a href=""{partnerUrl}"">Croquet Australia website</a>");

            return html;
        }

        private static string GetPartnerUrl(string baseUrl, Tournament tournament, EntrySubmitted @event)
        {
            return string.Format($"{baseUrl}/tournaments/{tournament.Starts.Year}/{tournament.Discipline}/{tournament.Slug}?FirstName={EncodeQueryStringValue(@event.Partner?.FirstName)}&LastName={EncodeQueryStringValue(@event.Partner?.LastName)}&Email={EncodeQueryStringValue(@event.Partner?.Email)}");
        }

        private static string EncodeQueryStringValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : Uri.EscapeDataString(value);
        }

        private static string FormatEventsClose(ZonedDateTime eventsClose)
        {
            var dateTimePattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd', 'd' 'MMMM' 'yyyy", DateTimeZoneProviders.Tzdb);
            var result = dateTimePattern.Format(eventsClose);

            return result;
        }

        private static string GetDeposited(EmailMessageSettings emailMessageSettings, EntrySubmitted entrySubmitted)
        {
            return $"{emailMessageSettings.BaseUrl}/tournaments/deposited?id={entrySubmitted.EntityId}";
        }

        private static string GetStating(EntrySubmitted entrySubmitted, Tournament tournament)
        {
            return $"{entrySubmitted.Player.FirstName.Substring(0, 1)} {entrySubmitted.Player.LastName} {tournament.DepositStating}";
        }

        private static decimal GetTotalPayable(EntrySubmitted entrySubmitted, TournamentItem tournamentEvent, Tournament tournament)
        {
            var functions = entrySubmitted.Functions.Sum(function => function.TotalPrice);
            var total = functions + GetTournamentTotalPrice(tournamentEvent, entrySubmitted, tournament);

            return total;
        }

        private static string GetFunctions(EntrySubmitted entrySubmitted, Tournament tournament)
        {
            var lineItems = entrySubmitted.Functions.ToArray();
            var lines = lineItems.Select(lineItem =>
            {
                var function = tournament.Functions.Single(f => f.Id == lineItem.Id);

                return $"{lineItem.Quantity} * {function.Title} {lineItem.TotalPrice:C}";
            });

            return string.Join(Environment.NewLine, lines);
        }

        private static decimal GetTournamentTotalPrice(TournamentItem tournamentEvent, EntrySubmitted @event, Tournament tournament)
        {
            if (tournamentEvent == null)
            {
                return 0;
            }

            var totalPrice = GetIndividualTournamentPrice(@event.Player, tournamentEvent);

            if (tournament.IsDoubles && @event.PayingForPartner)
            {
                totalPrice += GetIndividualTournamentPrice(@event.Partner, tournamentEvent);
            }

            return totalPrice;
        }

        private static decimal GetIndividualTournamentPrice(SubmitEntry.PlayerClass player, TournamentItem tournamentEvent)
        {
            var unitPrice = tournamentEvent.UnitPrice;
            var applyDiscount = player.FullTimeStudentUnder25 || player.Under21;
            var result = applyDiscount ? unitPrice / 2 : unitPrice;

            return result;
        }
    }
}