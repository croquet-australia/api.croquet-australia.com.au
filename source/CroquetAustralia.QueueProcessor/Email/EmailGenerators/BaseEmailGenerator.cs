using System.Reflection;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Library.Extensions;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    // todo: replace multiple classes with 'TemplateNameFinder' class
    public abstract class BaseEmailGenerator
    {
        protected readonly EmailMessageSettings EmailMessageSettings;

        protected BaseEmailGenerator(EmailMessageSettings emailMessageSettings)
        {
            EmailMessageSettings = emailMessageSettings;
        }

        public EmailMessage Generate(SubmitEntry.PlayerClass sendTo, EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            var templateName = GetTemplateName(entrySubmitted);
            var template = GetTemplate(templateNamespace, templateName);
            var emailMessage = new EmailMessage(EmailMessageSettings, tournament, template, entrySubmitted, sendTo);

            return emailMessage;
        }

        protected string GetTemplate(string templateNamespace, string templateName)
        {
            var resourceName = $"{templateNamespace}.{templateName}.txt";
            var assembly = Assembly.GetExecutingAssembly();
            var template = assembly.GetResourceText(resourceName);

            return template;
        }

        protected abstract string GetTemplateName(EntrySubmitted entrySubmitted);
    }
}