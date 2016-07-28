using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CroquetAustralia.Library.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailMessageSettings : BaseAppSettings
    {
        public EmailMessageSettings() : base("EmailMessage")
        {
        }

        public DirectoryInfo Attachments => GetAttachmentsDirectory();

        public EmailAddress[] Bcc => GetBcc().ToArray();
        public string BaseUrl => Get(nameof(BaseUrl));

        private DirectoryInfo GetAttachmentsDirectory()
        {
            const string appSettingKey = "AttachmentsDirectory";
            var path = Get(appSettingKey, true, true);

            if (!string.IsNullOrWhiteSpace(path))
            {
                return new DirectoryInfo(path);
            }

            path = Environment.GetEnvironmentVariable("TEMP");

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"AppSettings[{GetFullKey(appSettingKey)}] or environment variable 'TEMP' must be defined.");
            }

            return new DirectoryInfo(path);
        }

        private IEnumerable<EmailAddress> GetBcc()
        {
            var setting = Get(nameof(Bcc));

            if (setting == "none")
            {
                yield break;
            }

            var json = JsonConvert.DeserializeObject<JObject[]>(setting);

            foreach (var item in json)
            {
                yield return new EmailAddress(item["email"].Value<string>(), item["name"].Value<string>());
            }
        }
    }
}