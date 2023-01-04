using APS.DotNetSDK.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace APS.DotNetSDK.Tests.Logging
{
    public class SerilogExtensionsTests
    {
        [Test]
        public void AddSerilogLogger_EmptyConfiguration_AddsILogger()
        {
            // arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // act
            serviceCollection.AddSerilogLogger(configuration);
            var actual = serviceCollection.BuildServiceProvider().GetService<ILogger<SerilogExtensionsTests>>();

            // asert
            Assert.IsTrue(serviceCollection.Count(x => x.ServiceType == typeof(ILoggerFactory)) == 1);
            Assert.IsAssignableFrom<Logger<SerilogExtensionsTests>>(actual);
        }

        [Test]
        public void AddSerilogLogger_EmptyConfiguration_AddsILoggerProvider()
        {
            // arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // act
            serviceCollection.AddSerilogLogger(configuration);

            // asert
            Assert.IsTrue(serviceCollection.Count(x => x.ServiceType == typeof(ILoggerFactory)) == 1);
        }

        [Test]
        public void AddSerilogLogger_EmptyConfiguration_AddsILoggerFactory()
        {
            // arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            // act
            serviceCollection.AddSerilogLogger(configuration);

            // asert
            Assert.IsTrue(serviceCollection.Count(x => x.ServiceType == typeof(ILoggerFactory)) == 1);
        }

        [Test]
        public void AddSerilogLogger_Default_AddsILogger()
        {
            // arrange
            var serviceCollection = new ServiceCollection();

            // act
            serviceCollection.AddSerilogLogger("Logging/Config/SerilogConfig.json");
            var actual = serviceCollection.BuildServiceProvider().GetService<ILogger<SerilogExtensionsTests>>();

            // asert
            Assert.IsTrue(serviceCollection.Count(x => x.ServiceType == typeof(ILoggerFactory)) == 1);
            Assert.IsAssignableFrom<Logger<SerilogExtensionsTests>>(actual);
        }
    }
}