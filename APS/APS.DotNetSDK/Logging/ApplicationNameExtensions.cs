using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace APS.DotNetSDK.Logging
{
    public static class ApplicationNameExtensions
    {
        public const string ApplicationNameVariable = "$APPLICATIONNAME";

        public static string GetDefaultApplicationName()
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            return applicationName;
        }

        public static IConfiguration ReplaceApplicationNameVariable(this IConfiguration configuration, string applicationName = null)
        {
            applicationName = !string.IsNullOrEmpty(applicationName) ? applicationName : GetDefaultApplicationName();

            if (!string.IsNullOrEmpty(applicationName))
            {
                foreach (var keyValuePair in configuration.AsEnumerable(makePathsRelative: true).Where(c => !string.IsNullOrEmpty(c.Value) && c.Value.Contains(ApplicationNameVariable)))
                {
                    configuration[keyValuePair.Key] = keyValuePair.Value.Replace(ApplicationNameVariable, applicationName);
                }
            }

            return configuration;
        }
    }
}