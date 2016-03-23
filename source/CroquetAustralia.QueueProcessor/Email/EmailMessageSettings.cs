using System.Collections.Generic;
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

        public EmailAddress[] Bcc => GetBcc().ToArray();
        public string BaseUrl => Get(nameof(BaseUrl));

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