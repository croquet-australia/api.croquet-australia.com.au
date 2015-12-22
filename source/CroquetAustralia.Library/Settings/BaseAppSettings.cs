using System;
using System.Configuration;
using System.Linq;
using EmptyStringGuard;
using NullGuard;

namespace CroquetAustralia.Library.Settings
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

        [return: AllowNull]
        public string Get(string key, bool allowNullOrEmptyValue = false)
        {
            var fullKey = _appSettingsPrefix + key;
            var value = ConfigurationManager.AppSettings[fullKey];

            if (value != null)
            {
                return value;
            }
            if (!ConfigurationManager.AppSettings.AllKeys.Any(c => c.Equals(fullKey, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"AppSettings[{fullKey}] must be defined.");
            }
            if (allowNullOrEmptyValue)
            {
                return null;
            }
            throw new Exception($"AppSetting[{fullKey}] cannot be null.");
        }

        protected bool GetBoolean(string key)
        {
            var stringValue = Get(key);
            var booleanValue = bool.Parse(stringValue);

            return booleanValue;
        }
    }
}