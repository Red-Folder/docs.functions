using System.Configuration;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.Common.Helpers
{
    public class AppSettings
    {
        private const string APPSETTINGSPATH = @"..\..\appsettings.json";

        public static bool AppSetting(string key, bool defaultValue)
        {
            var value = AppSetting(key);
            return (value == null) ? defaultValue : bool.Parse(value);
        }

        public static string AppSetting(string key, string defaultValue)
        {
            var value = AppSetting(key);
            return (value == null) ? defaultValue : value;
        }

        public static string AppSetting(string key)
        {
            var value = FromEnvironment(key);

            if (value == null)
            {
                return FromAppsettingsFile(key);
            }

            return value;
        }

        private static string FromEnvironment(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }

        private static string FromAppsettingsFile(string key)
        {
            if (File.Exists(APPSETTINGSPATH))
            {
                var appsettings = File.ReadAllText(APPSETTINGSPATH);
                var settings = JObject.Parse(appsettings);
                return (string)settings[key];
            }
            else
            {
                return null;
            }
        }
    }
}
