using System;
using Microsoft.Extensions.DependencyInjection;

namespace APS.DotNetSDK.Configuration
{
    public class LoggingConfiguration
    {
        public LoggingConfiguration(IServiceCollection serviceCollection, string jsonLoggingPathConfig, string applicationName)
        {
            ServiceCollection = serviceCollection;
            JsonLoggingPathConfig = jsonLoggingPathConfig;
            ApplicationName = applicationName;

            Validate();
        }

        public IServiceCollection ServiceCollection { get; set; }
        public string JsonLoggingPathConfig { get; set; }
        public string ApplicationName { get; set; }

        public void Validate()
        {
            if(ServiceCollection == null)
            {
                throw new ArgumentNullException($"ServiceCollection", "ServiceCollection is needed for logging configuration");
            }  
            
            if(string.IsNullOrEmpty(JsonLoggingPathConfig))
            {
                throw new ArgumentNullException($"JsonLoggingPathConfig", "JsonLoggingPathConfig is needed for logging configuration");
            }  
            
            if(string.IsNullOrEmpty(ApplicationName))
            {
                throw new ArgumentNullException($"ApplicationName", "ApplicationName is needed for logging configuration");
            }  
        }
    }
}
