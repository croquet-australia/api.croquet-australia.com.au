using CroquetAustralia.Library.Settings;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class MailInBlueSettings : BaseAppSettings
    {
        public MailInBlueSettings() : base("MailInBlue")
        {
        }

        public string AccessId => Get("AccessId");
    }
}