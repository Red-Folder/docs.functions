using System.Configuration;
using System.Linq;

namespace Tests.Common.Helpers
{
    public class AppSettings
    {
        public static bool AppSetting(string key, bool defaultValue)
        {
            var value = GetSetting(key);
            return (value == null) ? defaultValue : bool.Parse(value);
        }

        public static string AppSetting(string key, string defaultValue)
        {
            var value = GetSetting(key);
            return (value == null) ? defaultValue : value;
        }

        private static string GetSetting(string key)
        {
            var value = FromEnvironment(key);

            if (value == null)
            {
                if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == key))
                {
                    value = ConfigurationManager.AppSettings[key];
                }
            }

            return value;
        }

        private static string FromEnvironment(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }
    }
}
