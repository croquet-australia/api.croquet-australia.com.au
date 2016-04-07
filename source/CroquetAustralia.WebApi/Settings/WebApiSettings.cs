namespace CroquetAustralia.WebApi.Settings
{
    public class WebApiSettings : BaseAppSettings
    {
        public WebApiSettings()
            : base("WebApi")
        {
        }

        public bool RunningTests => GetBoolean(nameof(RunningTests), true);
    }
}