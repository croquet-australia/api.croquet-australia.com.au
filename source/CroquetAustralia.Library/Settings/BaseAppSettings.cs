using System;
using System.Configuration;
using System.Linq;
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
        public string Get(string key, bool allowNullOrEmptyValue = false, bool allowUndefined = false)
        {
            var fullKey = GetFullKey(key);
            var value = GetEnvironmentVariableValue(fullKey);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = ConfigurationManager.AppSettings[fullKey];

            if (value != null)
            {
                return value;
            }
            if (!ConfigurationManager.AppSettings.AllKeys.Any(c => c.Equals(fullKey, StringComparison.OrdinalIgnoreCase)))
            {
                if (allowUndefined)
                {
                    return null;
                }
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

        protected string GetFullKey(string key)
        {
            return _appSettingsPrefix + key;
        }

        private static string GetEnvironmentVariableValue(string appSettingsKey)
        {
            var environmentVariableName = $"APPSETTING_{appSettingsKey}";
            var value = Environment.GetEnvironmentVariable(environmentVariableName);

            return value;
        }
    }
}