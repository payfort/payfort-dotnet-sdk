using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace APS.DotNetSDK.Logging
{
    public static class HostnameExtensions
    {
        public const string HostnameKey = "HOSTNAME";
        public const string ComputerNameKey = "COMPUTERNAME";
        public const string HostnameVariable = "$HOSTNAME";

        public static string GetHostname()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            var hostname = environmentVariables[HostnameKey]?.ToString() ?? environmentVariables[ComputerNameKey]?.ToString();
            return hostname;
        }

        public static IConfiguration ReplaceHostnameVariable(this IConfiguration configuration)
        {
            var hostname = GetHostname();

            if (!string.IsNullOrEmpty(hostname))
            {
                foreach (var keyValuePair in configuration.AsEnumerable(makePathsRelative: true).Where(c => !string.IsNullOrEmpty(c.Value) && c.Value.Contains(HostnameVariable)))
                {
                    configuration[keyValuePair.Key] = keyValuePair.Value.Replace(HostnameVariable, hostname);
                }
            }

            return configuration;
        }
    }
}