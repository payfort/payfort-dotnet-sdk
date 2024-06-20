using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Configuration
{
    public class SdkConfigurationTests
    {
        private readonly string _filePath = $"Web{Path.DirectorySeparatorChar.ToString()}ApplePayIntegration{Path.DirectorySeparatorChar.ToString()}Certificate.pem";
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly Mock<ILoggerFactory> _loggerFactory = new Mock<ILoggerFactory>();


        [Test]
        public void Configure_ReturnError_WhenFilePathIsWrong()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissing1.json";

            var expectedException = "The file \"MerchantSdkConfiguration.json\" is needed for SDK configuration";
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void Configure_ReturnNoError_InputIsValids()
        {
            SdkConfiguration.ClearConfiguration();
            //arrange
            //act
            //assert
            Assert.DoesNotThrow(() => SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object));
        }

        [Test]
        public void Configure_ReturnError_AccessCodeIsNull()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissing.json";

            var expectedException = "AccessCode is needed for SDK configuration (Parameter 'AccessCode')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ReturnError_IsTestEnvironmentIsSentIncorrect()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithIsTestEnvironmentSentIncorrect.json";

            var expectedException = "Please provide one of IsTestEnvironment \"Test\" or \"Production\". " +
                        "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ReturnError_MerchantIdentifierIsNull()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithMerchantIdentifierMissing.json";

            var expectedException = "MerchantIdentifier is needed for SDK configuration (Parameter 'MerchantIdentifier')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ReturnError_RequestShaPhraseIsNull()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithRequestShaPhraseMissing.json";

            var expectedException = "RequestShaPhrase is needed for SDK configuration (Parameter 'RequestShaPhrase')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ReturnError_ResponseShaPhraseIsNull()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithResponseShaPhraseMissing.json";

            var expectedException = "ResponseShaPhrase is needed for SDK configuration (Parameter 'ResponseShaPhrase')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ReturnError_ShaTypeIsSentIncorrect()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithShaTypeSentIncorrect.json";

            var expectedException = "Please provide one of the shaType \"Sha512\" or \"Sha256\". " +
                        "Is needed in Sdk Configuration. Please check file \"MerchantSdkConfiguration.json\"";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ValidateIsSuccessful_ReturnNoError()
        {
            SdkConfiguration.ClearConfiguration();
            //arrange
            //act
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object);

            //assert
            Assert.That(SdkConfiguration.IsConfigured, Is.True);
            Assert.DoesNotThrow(() => SdkConfiguration.Validate());
        }

        [Test]
        public async Task Configure_ApplePayConfigurationIsNotNull_ReturnNoError()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));
            SdkConfiguration.ClearConfiguration();
            //arrange
            //act
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object);

            //assert
            Assert.That(SdkConfiguration.IsConfigured, Is.True);
            Assert.DoesNotThrow(() => SdkConfiguration.Validate());
        }

        [Test]
        public async Task Configure_DisplayNameFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithDisplayNameMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "DisplayName is needed for ApplePay configuration (Parameter 'DisplayName')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, _loggerFactory.Object));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_AccessCodeFromApplePayIsMissing_ThrowsException()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissingForApplePay.json";

            var expectedException = "AccessCode is needed for ApplePay configuration (Parameter 'AccessCode')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, _loggerFactory.Object));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_RequestShaPhraseFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithRequestShaPhraseMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "RequestShaPhrase is needed for ApplePay configuration (Parameter 'RequestShaPhrase')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, _loggerFactory.Object));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_ResponseShaPhraseFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithResponseShaPhraseMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "ResponseShaPhrase is needed for ApplePay configuration (Parameter 'ResponseShaPhrase')";
            SdkConfiguration.ClearConfiguration();
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, _loggerFactory.Object));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_WhenCertificateIsSent_ReturnNoError()
        {
            SdkConfiguration.ClearConfiguration();
            //arrange
            //act
            //assert
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));
            Assert.DoesNotThrow(() =>
                SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object, new ApplePayConfiguration(certificate)));
        }

        [Test]
        public void Configure_WhenCertificateIsNull_ReturnError()
        {
            SdkConfiguration.ClearConfiguration();
            //arrange
            //act
            //assert
            const string expectedException = "SecurityCertificate is needed for ApplePay configuration (Parameter 'SecurityCertificate')";
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object, new ApplePayConfiguration(null)));

            Assert.That(actualException?.Message, Is.EqualTo(expectedException));
        }
    }
}
