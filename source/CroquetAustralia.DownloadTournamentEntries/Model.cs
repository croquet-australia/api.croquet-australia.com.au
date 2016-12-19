using System;
using System.Collections.Generic;
using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.DownloadTournamentEntries.ReadModels;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CroquetAustralia.DownloadTournamentEntries
{
    public class Model
    {
        public Model(string partitionKey, IEnumerable<DynamicTableEntity> entities, ITournamentsRepository tournamentsRepository)
        {
            Id = partitionKey;

            foreach (var entity in entities.OrderBy(g => g.Timestamp))
            {
                var eventType = entity["EventType"].StringValue;
                var eventJson = entity["Event"].StringValue;

                switch (eventType)
                {
                    case "CroquetAustralia.Domain.Features.TournamentEntry.Events.EntrySubmitted":
                        ApplyEntrySubmitted(eventJson, tournamentsRepository);
                        break;

                    case "CroquetAustralia.Domain.Features.TournamentEntry.Events.SentEntrySubmittedEmail":
                        ApplySentEntrySubmittedEmail(eventJson);
                        break;

                    case "CroquetAustralia.Domain.Features.TournamentEntry.Events.PaymentReceived":
                        ApplyPaymentReceived(eventJson);
                        break;

                    default:
                        throw new NotSupportedException($"Event type '{eventType}' is not supported.");
                }
            }
        }

        public string Id { get; }
        public EntrySubmittedReadModel EntrySubmitted { get; private set; }
        public SentEntrySubmittedEmailReadModel SentEntrySubmittedEmail { get; private set; }
        public PaymentReceivedReadModel PaymentReceived { get; private set; }

        public static string Headings => string.Join(",",
            "Tournament",
            "Entry Id",
            "First Name",
            "Last Name",
            "Email",
            "Phone",
            "Payment Method",
            "Event",
            "Under 21",
            "Full Time Student Under 25",
            "Entry Submitted (AEST)",
            "Payment Sent (AEST)",
            "Handicap",
            "Paying for Partner",
            "Partner",
            "Partner Handicap",
            "Entry Email Sent (AEST)",
            "Entry Email Id", 
            "GC DGrade",
            "Country");

        private void ApplyEntrySubmitted(string eventJson, ITournamentsRepository tournamentsRepository)
        {
            var @event = JsonConvert.DeserializeObject<EntrySubmitted>(eventJson);
            var tournament = tournamentsRepository.GetByIdAsync(@event.TournamentId).Result;

            EntrySubmitted = new EntrySubmittedReadModel(@event, tournament);
        }

        private void ApplySentEntrySubmittedEmail(string eventJson)
        {
            var @event = JsonConvert.DeserializeObject<SentEntrySubmittedEmail>(eventJson);

            SentEntrySubmittedEmail = new SentEntrySubmittedEmailReadModel(@event);
        }

        private void ApplyPaymentReceived(string eventJson)
        {
            var @event = JsonConvert.DeserializeObject<PaymentReceived>(eventJson);

            PaymentReceived = new PaymentReceivedReadModel(@event);
        }

        public override string ToString()
        {
            var values = new object[]
            {
                EntrySubmitted.Tournament.Title,
                Id,
                EntrySubmitted.Player.FirstName,
                EntrySubmitted.Player.LastName,
                EntrySubmitted.Player.Email,
                FormatPhone(EntrySubmitted.Player.Phone),
                EntrySubmitted.PaymentMethod,
                EntrySubmitted.Event?.Title,
                EntrySubmitted.Player.Under21,
                EntrySubmitted.Player.FullTimeStudentUnder25,
                FormatTimeStamp(EntrySubmitted.Created),
                FormatTimeStamp(PaymentReceived?.Created),
                EntrySubmitted.Player.Handicap,
                EntrySubmitted.Tournament.IsDoubles ? EntrySubmitted.PayingForPartner.ToString() : "",
                EntrySubmitted.Partner == null ? null : string.Format($"{EntrySubmitted.Partner.FirstName} {EntrySubmitted.Partner.LastName}"),
                EntrySubmitted.Partner?.Handicap,
                FormatTimeStamp(SentEntrySubmittedEmail?.Created),
                SentEntrySubmittedEmail?.EmailId,
                EntrySubmitted.Player.GCDGrade,
                EntrySubmitted.Player.Country
            };

            return string.Join(",", values.Select(FormatValue));
        }

        private static int? AttendingFunction(IEnumerable<FunctionReadModel> functions, string functionId)
        {
            return FindFunction(functions, functionId)?.Quantity;
        }

        private static FunctionReadModel FindFunction(IEnumerable<FunctionReadModel> functions, string functionId)
        {
            return functions.SingleOrDefault(function => function.Id.ToString() == functionId);
        }

        private static string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return null;
            }

            phone = phone.Trim();

            return phone.Length != 10 ? phone : $"{phone.Substring(0, 4)} {phone.Substring(4, 3)} {phone.Substring(7, 3)}";
        }

        private static string FormatTimeStamp(DateTime? utcDateTime)
        {
            return utcDateTime.HasValue ? FormatTimeStamp(utcDateTime.Value) : null;
        }

        private static string FormatTimeStamp(DateTime utcDateTime)
        {
            var aestTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            var aestDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, aestTimeZoneInfo);
            var formatted = aestDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return formatted;
        }

        private static object FormatValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.GetType() != typeof(string)
                ? value
                : $@"""{value.ToString().Trim()}""";
        }
    }
}