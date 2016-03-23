using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalTests.Writers;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators;
using CroquetAustralia.QueueProcessor.Processors;
using CroquetAustralia.QueueProcessor.UnitTests.TestHelpers;
using CroquetAustralia.QueueProcessor.UnitTests.TestHelpers.Mocks;
using CroquetAustralia.TestHelpers;
using Xunit;

namespace CroquetAustralia.QueueProcessor.UnitTests.WebJobs
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

        [UseReporter(typeof(DiffReporter))]
        // ReSharper disable once InconsistentNaming
        public abstract class ProcessEventAsync_PayByType : SendEntrySubmittedEmailQueueProcessorTests
        {
            private readonly InMemoryEmailService _emailService;
            private readonly PaymentMethod _paymentMethod;
            private readonly SendEntrySubmittedEmailQueueProcessor _processor;
            private readonly TournamentsRepository _tournamentsRepository;

            protected ProcessEventAsync_PayByType(PaymentMethod paymentMethod)
            {
                _paymentMethod = paymentMethod;
                _emailService = new InMemoryEmailService();

                // todo: InMemory repository
                _tournamentsRepository = new TournamentsRepository();

                var queueMessageSerializer = new QueueMessageSerializer();
                var eventsQueue = new InMemoryEventQueue();
                var emailMessageSettings = new EmailMessageSettings();
                var singlesEmailGenerator = new SinglesEmailGenerator(emailMessageSettings);
                var doublesPlayerEmailGenerator = new DoublesPlayerEmailGenerator(emailMessageSettings);
                var doublesPartnerEmailGenerator = new DoublesPartnerEmailGenerator(emailMessageSettings);
                var emailGenerator = new EmailGenerator(_tournamentsRepository, singlesEmailGenerator, doublesPlayerEmailGenerator, doublesPartnerEmailGenerator);

                _processor = new SendEntrySubmittedEmailQueueProcessor(
                    queueMessageSerializer,
                    eventsQueue,
                    _emailService,
                    emailGenerator);
            }

            [Fact]
            public void ShouldSendEmailWhenEventAndFunctionsAreSelected()
            {
                ShouldSendEmail(true, Functions());
            }

            [Fact]
            public void ShouldSendEmailWhenOnlyEventIsSelected()
            {
                ShouldSendEmail(true, new SubmitEntry.LineItem[] {});
            }

            [Fact]
            public void ShouldSendEmailWhenOnlyFunctionsAreSelected()
            {
                ShouldSendEmail(false, Functions());
            }

            private void ShouldSendEmail(bool eventSelected, SubmitEntry.LineItem[] functions)
            {
                // Given
                var tournament = _tournamentsRepository.GetAllAsync().Result.First();
                var @event = new EntrySubmitted
                {
                    EntityId = new Guid("d99dd017-edab-46a6-bb58-fde7c6c619fa"),
                    EventId = eventSelected ? tournament.Events.First().Id : (Guid?)null,
                    PaymentMethod = _paymentMethod,
                    TournamentId = tournament.Id,
                    Player = new SubmitEntry.PlayerClass
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

            private static string GetApprovalName(bool eventSelected, IEnumerable<SubmitEntry.LineItem> functions)
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

            private SubmitEntry.LineItem[] Functions()
            {
                return new[]
                {
                    new SubmitEntry.LineItem
                    {
                        DiscountPercentage = 0,
                        Id = _tournamentsRepository.GetAllAsync().Result.First().Functions.Last().Id,
                        Quantity = 1,
                        UnitPrice = 50
                    }
                };
            }
        }
    }
}