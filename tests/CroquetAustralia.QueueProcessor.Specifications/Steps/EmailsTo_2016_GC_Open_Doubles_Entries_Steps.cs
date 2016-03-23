using System;
using System.IO;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Writers;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Specifications.TestHelpers;
using CroquetAustralia.TestHelpers;
using TechTalk.SpecFlow;

namespace CroquetAustralia.QueueProcessor.Specifications.Steps
{
    [Binding]
    [UseReporter(typeof(DiffReporter))]
    public class EmailsTo_2016_GC_Open_Doubles_Entries_Steps : TestBase
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly IServiceProvider _services;

        public EmailsTo_2016_GC_Open_Doubles_Entries_Steps(GivenData given, ActualData actual)
            : this(given, actual, TestHelpers.ServiceProvider.Instance)
        {
        }

        private EmailsTo_2016_GC_Open_Doubles_Entries_Steps(GivenData given, ActualData actual, IServiceProvider services)
        {
            _given = given;
            _actual = actual;
            _services = services;
        }

        [Given(@"tournament slug is '(.*)'")]
        public void GivenTournamentSlugIs(string tournamentSlug)
        {
            _given.TournamentSlug = tournamentSlug;
        }

        [Given(@"payingForPartner is '(.*)'")]
        public void GivenPayingForPartnerIs(bool payingForPartner)
        {
            _given.PayingForPartner = payingForPartner;
        }

        [Given(@"paymentMethod is '(.*)'")]
        public void GivenPaymentMethodIs(string paymentMethod)
        {
            _given.PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), paymentMethod, true);
        }

        [When(@"the entry is submitted")]
        public void WhenTheEntryIsSubmitted()
        {
            var submitEntry = Valid<SubmitEntry>();

            var tournament = _services.Get<ITournamentsRepository>().GetByUrlAsync(_given.TournamentSlug).Result;

            submitEntry.EntityId = Guid.Parse("28e4df74-1acb-4a2e-a317-63f064f10142");
            submitEntry.EventId = tournament.Events.First().Id;
            submitEntry.Functions = new SubmitEntry.LineItem[] {};
            submitEntry.Merchandise = new SubmitEntry.LineItem[] {};
            submitEntry.PaymentMethod = _given.PaymentMethod;
            submitEntry.PayingForPartner = _given.PayingForPartner;
            submitEntry.TournamentId = tournament.Id;

            submitEntry.Player = new SubmitEntry.PlayerClass
            {
                Email = "joe@example.com",
                FirstName = "Joe",
                LastName = "Blow"
            };

            submitEntry.Partner = new SubmitEntry.PlayerClass
            {
                Email = "jane@example.com",
                FirstName = "Jane",
                LastName = "Doe"
            };

            _given.EntrySubmitted = submitEntry.ToEntrySubmitted();
            _actual.Emails = _services.Get<IEmailGenerator>().GenerateAsync(_given.EntrySubmitted).Result.ToArray();
        }

        [Then(@"an email using '(.*)' template is sent to the player")]
        public void ThenAnEmailUsingTemplateIsSentToThePlayer(string templateFileName)
        {
            VerifyEmailMessageTo(_given.EntrySubmitted.Player, templateFileName);
        }

        [Then(@"an email using '(.*)' template is sent to the partner")]
        public void ThenAnEmailUsingTemplateIsSentToThePartner(string templateFileName)
        {
            VerifyEmailMessageTo(_given.EntrySubmitted.Partner, templateFileName);
        }

        private void VerifyEmailMessageTo(SubmitEntry.PlayerClass player, string templateFileName)
        {
            var emailMessage = _actual.Emails.Single(e => e.To.Single().Email == player.Email);
            var actual = emailMessage.ToApprovalText();

            var writer = WriterFactory.CreateTextWriter(actual);
            var namer = new ApprovalNamer(Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\Steps\PayBy{_given.PaymentMethod}"), templateFileName);
            var reporter = Approvals.GetReporter();

            Approvals.Verify(writer, namer, reporter);
        }

        [Given(@"todo")]
        public void GivenTodo()
        {
            throw new NotImplementedException("todo");
        }
    }
}