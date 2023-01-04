using APS.DotNetSDK.Logging;
using Microsoft.Extensions.Configuration;


namespace APS.DotNetSDK.Tests.Logging
{
    public class ApplicationNameExtensionsTests
    {
        private const string DefaultApplicationName = "testhost";
        private const string ApplicationName = "Application Name";
        private static readonly Dictionary<string, string> ConfigWithoutApplicationName =
            new()
            {
                {"key1", "value1"},
                {"key2", "value2"}
            };

        private static readonly Dictionary<string, string> ConfigWithApplicationName =
            new Dictionary<string, string>
            {
                {"key1", "$APPLICATIONNAME-value1"},
                {"key2", "value2 $APPLICATIONNAME"}
            };

        [Test]
        public void GetApplicationName_ReturnsApplicationNameValue()
        {
            // act
            var actual = ApplicationNameExtensions.GetDefaultApplicationName();

            // assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void ReplaceApplicationNameVariable_ApplicationNameVariableDoesNotExistInConfiguration_MakesNoChanges()
        {
            // arrange
            var expectedKey1 = ConfigWithoutApplicationName["key1"];
            var expectedKey2 = ConfigWithoutApplicationName["key2"];
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithoutApplicationName).Build();

            // act
            configuration.ReplaceApplicationNameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }

        [Test]
        public void ReplaceApplicationNameVariable_ApplicationNameVariableExistsInConfiguration_ReplacesVariableWithDefaultApplicationName()
        {
            // arrange
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithApplicationName).Build();

            // act
            configuration.ReplaceApplicationNameVariable();
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.IsTrue(actualKey1.EndsWith(ConfigWithoutApplicationName["key1"]));
            Assert.IsTrue(actualKey2.StartsWith(ConfigWithoutApplicationName["key2"]));
        }

        [Test]
        public void ReplaceApplicationNameVariable_ApplicationNameVariableExistsInConfiguration_ReplacesVariableWithDefinedApplicationName()
        {
            // arrange
            var expectedKey1 = ConfigWithApplicationName["key1"].Replace(ApplicationNameExtensions.ApplicationNameVariable, ApplicationName);
            var expectedKey2 = ConfigWithApplicationName["key2"].Replace(ApplicationNameExtensions.ApplicationNameVariable, ApplicationName);
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(ConfigWithApplicationName).Build();

            // act
            configuration.ReplaceApplicationNameVariable(ApplicationName);
            var actualKey1 = configuration["key1"];
            var actualKey2 = configuration["key2"];

            // assert
            Assert.That(actualKey1, Is.EqualTo(expectedKey1));
            Assert.That(actualKey2, Is.EqualTo(expectedKey2));
        }
    }
}