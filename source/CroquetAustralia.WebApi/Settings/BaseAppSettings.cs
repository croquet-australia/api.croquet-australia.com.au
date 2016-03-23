using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;

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
                throw new Exception($"Value for AppSetting {_appSettingsPrefix + key} cannot be null.");
            }
            throw new Exception($"AppSettings[{_appSettingsPrefix}] is empty. Maybe you need to create BaseAppSettings.config. See BaseAppSettings.Example.config.");
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