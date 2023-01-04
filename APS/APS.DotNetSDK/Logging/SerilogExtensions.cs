using System;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APS.DotNetSDK.Logging
{
    public static class SerilogExtensions
    {
        private const string SerilogConfigSectionKey = "Serilog";

        /// <summary>
        /// Adds a Serilog logger to the specified IServiceCollection with the default sinks defined in <param name="jsonFilePath"></param>.
        /// </summary>
        ///<param name="serviceCollection"></param>
        /// <param name="jsonFilePath"></param>
        /// <param name="applicationName"></param>
        public static void AddSerilogLogger(this IServiceCollection serviceCollection, string jsonFilePath, string applicationName = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(jsonFilePath, optional: false, reloadOnChange: true)
                .Build();

            configuration.GetSection(SerilogConfigSectionKey).ReplaceApplicationNameVariable(applicationName);
            serviceCollection.AddSerilogLogger(configuration);
        }

        /// <summary>
        /// Adds a Serilog logger to the specified IServiceCollection with the sinks defined in the specified IConfiguration.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        public static void AddSerilogLogger(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            configuration.GetSection(SerilogConfigSectionKey).ReplaceHostnameVariable();

            var logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(configuration, SerilogConfigSectionKey)
                 .Enrich.FromLogContext()
                 .Enrich.WithMachineName()
                 .CreateLogger();

            serviceCollection.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(logger);
            });
        }
    }
}
