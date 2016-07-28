using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var attachments = GetAttachments(templateNamespace);
            var emailMessage = TournamentEntryEmailMessage.Create(EmailMessageSettings, tournament, template, entrySubmitted, sendTo, attachments);

            return emailMessage;
        }

        protected abstract string GetTemplateName(EntrySubmitted entrySubmitted);

        protected virtual IEnumerable<string> GetAttachmentNames()
        {
            yield break;
        }

        private IEnumerable<FileInfo> GetAttachments(string templateNamespace)
        {
            return GetAttachmentNames().Select(attachmentName => GetAttachment(templateNamespace, attachmentName));
        }

        private FileInfo GetAttachment(string templateNamespace, string attachmentName)
        {
            var attachment = new FileInfo(Path.Combine(EmailMessageSettings.Attachments.FullName, attachmentName));

            if (attachment.Exists)
            {
                return attachment;
            }

            SaveAssemblyResourceAsFile(templateNamespace, attachment);

            attachment.Refresh();

            return attachment;
        }

        private static void SaveAssemblyResourceAsFile(string templateNamespace, FileInfo attachment)
        {
            var resourceName = $"{templateNamespace}.{attachment.Name}";
            var assembly = Assembly.GetExecutingAssembly();

            assembly.SaveResourceAsFile(resourceName, attachment);
        }

        private static string GetTemplate(string templateNamespace, string templateName)
        {
            var resourceName = $"{templateNamespace}.{templateName}.txt";
            var assembly = Assembly.GetExecutingAssembly();
            var template = assembly.GetResourceText(resourceName);

            return template;
        }
    }
}