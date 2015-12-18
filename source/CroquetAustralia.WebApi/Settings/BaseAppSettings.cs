using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using EmptyStringGuard;

namespace CroquetAustralia.WebApi.Settings
{
    public abstract class BaseAppSettings
    {
        private readonly string _appSettingsPrefix;

        protected BaseAppSettings(string appSettingsPrefix)
        {
            _appSettingsPrefix = appSettingsPrefix;

            if (_appSettingsPrefix != "")
            {
                _appSettingsPrefix += ":";
            }
        }

        public string Get(string key)
        {
            var fullKey = _appSettingsPrefix + key;
            var value = WebConfigurationManager.AppSettings[fullKey];

            if (value != null)
            {
                return value;
            }
            if (WebConfigurationManager.AppSettings.AllKeys.Any(c => c.StartsWith(_appSettingsPrefix)))
            {
                throw new Exception(string.Format("Value for AppSetting {0} cannot be null.", _appSettingsPrefix + key));
            }
            throw new Exception(string.Format("AppSettings[{0}] is empty. Maybe you need to create BaseAppSettings.config. See BaseAppSettings.Example.config.", _appSettingsPrefix));
        }

        protected bool GetBoolean(string key)
        {
            var stringValue = Get(key);
            var booleanValue = bool.Parse(stringValue);

            return booleanValue;
        }

        public string GetDirectory(string key, HttpServerUtility server)
        {
            return server.MapPath(Get(key));
        }
    }
}