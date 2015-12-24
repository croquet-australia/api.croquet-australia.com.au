using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalTests.Writers;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.Domain.UnitTests.TestHelpers;
using CroquetAustralia.QueueProcessor.Processors;
using CroquetAustralia.QueueProcessor.Tests.TestHelpers;
using CroquetAustralia.QueueProcessor.Tests.TestHelpers.Mocks;
using Xunit;

namespace CroquetAustralia.QueueProcessor.Tests.WebJobs
{
    public class SendEntrySubmittedEmailQueueProcessorTests : TestBase
    {
        // ReSharper disable once InconsistentNaming
        public class ProcessEventAsync_PayByEFT : ProcessEventAsync_PayByType
        {
            public ProcessEventAsync_PayByEFT() : base(PaymentMethod.EFT)
            {
            }
        }

        // ReSharper disable once InconsistentNaming
        public class ProcessEventAsync_PayByCheque : ProcessEventAsync_PayByType
        {
            public ProcessEventAsync_PayByCheque() : base(PaymentMethod.Cheque)
            {
            }
        }

        [UseReporter(typeof (DiffReporter))]
        // ReSharper disable once InconsistentNaming
        public class ProcessEventAsync_PayByType : SendEntrySubmittedEmailQueueProcessorTests
        {
            private readonly PaymentMethod _paymentMethod;
            private readonly InMemoryEmailService _emailService;
            private readonly SendEntrySubmittedEmailQueueProcessor _processor;

            public ProcessEventAsync_PayByType(PaymentMethod paymentMethod)
            {
                _paymentMethod = paymentMethod;
                _emailService = new InMemoryEmailService();

                var queueMessageSerializer = new QueueMessageSerializer();
                var eventsQueue = new InMemoryEventQueue();

                _processor = new SendEntrySubmittedEmailQueueProcessor(queueMessageSerializer, eventsQueue, _emailService);
            }

            [Fact]
            public void ShouldSendEmailWhenEventAndFunctionsAreSelected()
            {
                ShouldSendEmail(true, Functions());
            }

            [Fact]
            public void ShouldSendEmailWhenOnlyEventIsSelected()
            {
                ShouldSendEmail(true, new LineItem[] { });
            }

            [Fact]
            public void ShouldSendEmailWhenOnlyFunctionsAreSelected()
            {
                ShouldSendEmail(false, Functions());
            }

            private void ShouldSendEmail(bool eventSelected, LineItem[] functions)
            {
                // Given
                var @event = new EntrySubmitted
                {
                    EntityId = new Guid("d99dd017-edab-46a6-bb58-fde7c6c619fa"),
                    EventId = eventSelected ? Tournaments.MensOpen.Events.First().Id : (Guid?) null,
                    PaymentMethod = _paymentMethod,
                    TournamentId = Tournaments.MensOpen.Id,
                    Player = new Player
                    {
                        Email = "tim@example.com",
                        FirstName = "Tim",
                        LastName = "Murphy",
                        Phone = "555",
                        Handicap = -1
                    },
                    Functions = functions
                };

                // When
                Invoke(() => _processor.ProcessEventAsync(@event, @event.GetType(), new StringWriter()).Wait());

                // Then
                var sentEmail = _emailService.SentEmails.Single().Value;
                var approvalName = GetApprovalName(eventSelected, functions);

                Verify(approvalName, sentEmail);
            }

            private static string GetApprovalName(bool eventSelected, IEnumerable<LineItem> functions)
            {
                if (eventSelected)
                {
                    return functions.Any() ? "ShouldSendEmailWhenEventAndFunctionsAreSelected" : "ShouldSendEmailWhenOnlyEventIsSelected";
                }

                if (functions.Any())
                {
                    return "ShouldSendEmailWhenOnlyFunctionsAreSelected";
                }

                throw new Exception("Did not expected to get to this point!");
            }

            private void Verify(string name, InMemorySentEmail sentEmail)
            {
                var sentEmailText = sentEmail.ToApprovalText();

                var writer = WriterFactory.CreateTextWriter(sentEmailText);
                var namer = new ApprovalNamer(GetApprovalSourcePath(), name);
                var reporter = Approvals.GetReporter();

                Approvals.Verify(writer, namer, reporter);
            }

            private string GetApprovalSourcePath()
            {
                var defaultPath = new UnitTestFrameworkNamer().SourcePath;
                return Path.Combine(defaultPath, $@"Approvals\ProcessEventAsync\PayBy{_paymentMethod}");
            }

            private static LineItem[] Functions()
            {
                return new[]
                {
                    new LineItem
                    {
                        DiscountPercentage = 0,
                        Id = Tournaments.MensOpen.Functions.Last().Id,
                        Quantity = 1,
                        UnitPrice = 50
                    }
                };
            }

        }
    }
}