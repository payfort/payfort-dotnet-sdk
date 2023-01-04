using APS.DotNetSDK.Logging;
using Microsoft.Extensions.Configuration;

namespace APS.DotNetSDK.Tests.Logging
{
    internal class HostnameExtensionsTests
    {
        private const string Hostname = "UnitTestHostname";
        private const string ComputerName = "UnitTestComputerName";
        private static readonly Dictionary<string, string> ConfigWithoutHostname =
            new()
            {
                {"key1", "value1"},
                {"key2", "value2"}
            };

        private static readonly Dictionary<string, string> ConfigWithHostname =
            new()
            {
                {"key1", "$HOSTNAME-value1"},
                {"key2", "value2 $HOSTNAME"}
            };

        [SetUp]
        public void HostnameExtensionsTestsSetup()
        {
            // arrange
            var expected = Hostname;
            Environment.SetEnvironmentVariable(HostnameExtensions.HostnameKey, Hostname);

            // act
            var actual = Environment.GetEnvironmentVariable(HostnameExtensions.HostnameKey);

            // assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetHostname_HostnameVariableSet_ReturnsHostnameValue()
        {
            // arrange
            var expected = Hostname;

            // act
            var actual = HostnameExtensions.GetHostname();

            // assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetHostname_ComputerNameVariableSet_ReturnsComputerNameValue()
        {
            // arrange
            var expected = ComputerName;
            Environment.SetEnvironmentVariable(HostnameExtensions.HostnameKey, null);
            Environment.SetEnvironmentVariable(HostnameExtensions.ComputerNameKey, ComputerName);

            // act
            var actual = HostnameExtensions.GetHostname();

            // assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceHostnameVariable_HostnameIsSetAndHostnameVariableDoesNotExistInConfiguration_MakesNoChanges()
        {
            // arrange
            var expectedKey1 = ConfigWithoutHostname["key1"];
            var expectedKey2 = ConfigWithoutHostname["key2"];
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithoutHostname).Build();

            // act
            configuration.ReplaceHostnameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }

        [Test]
        public void ReplaceHostnameVariable_HostnameIsNotSetAndHostnameVariableDoesNotExistInConfiguration_MakesNoChanges()
        {
            // arrange
            var expectedKey1 = ConfigWithoutHostname["key1"];
            var expectedKey2 = ConfigWithoutHostname["key2"];
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithoutHostname).Build();
            Environment.SetEnvironmentVariable(HostnameExtensions.HostnameKey, null);
            Environment.SetEnvironmentVariable(HostnameExtensions.ComputerNameKey, null);

            // act
            configuration.ReplaceHostnameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }

        [Test]
        public void ReplaceHostnameVariable_HostnameIsSetAndHostnameVariableExistsInConfiguration_ReplacesVariableWithFoundHostname()
        {
            // arrange
            var expectedKey1 = ConfigWithHostname["key1"].Replace(HostnameExtensions.HostnameVariable, Hostname);
            var expectedKey2 = ConfigWithHostname["key2"].Replace(HostnameExtensions.HostnameVariable, Hostname);
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithHostname).Build();

            // act
            configuration.ReplaceHostnameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }

        [Test]
        public void ReplaceHostnameVariable_HostnameIsNotSetAndHostnameVariableExistsInConfiguration_MakesNoChanges()
        {
            // arrange
            var expectedKey1 = ConfigWithHostname["key1"];
            var expectedKey2 = ConfigWithHostname["key2"];
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithHostname).Build();
            Environment.SetEnvironmentVariable(HostnameExtensions.HostnameKey, null);
            Environment.SetEnvironmentVariable(HostnameExtensions.ComputerNameKey, null);

            // act
            configuration.ReplaceHostnameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }
    }
}
