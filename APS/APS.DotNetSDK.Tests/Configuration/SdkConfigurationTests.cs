using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Configuration
{
    public class SdkConfigurationTests
    {
        private const string FilePath = @"Web\ApplePayIntegration\Certificate.pem";
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");


        [Test]
        public void Configure_ReturnError_WhenFilePathIsWrong()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissing1.json";

            var expectedException = "The file \"MerchantSdkConfiguration.json\" is needed for SDK configuration";
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void Configure_ReturnNoError_InputIsValids()
        {
            //arrange
            //act
            //assert
            Assert.DoesNotThrow(() => SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration));
        }

        [Test]
        public void Configure_ReturnError_AccessCodeIsNull()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissing.json";

            var expectedException = "AccessCode is needed for SDK configuration (Parameter 'AccessCode')";
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

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
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

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
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

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
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

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
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

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
            var actualException = Assert.Throws<SdkConfigurationException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_ValidateIsSuccessful_ReturnNoError()
        {
            //arrange
            //act
            SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration);

            //assert
            Assert.That(SdkConfiguration.IsConfigured, Is.True);
            Assert.DoesNotThrow(() => SdkConfiguration.Validate());
        }

        [Test]
        public async Task Configure_ApplePayConfigurationIsNotNull_ReturnNoError()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(FilePath));

            //arrange
            //act
            SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration);

            //assert
            Assert.That(SdkConfiguration.IsConfigured, Is.True);
            Assert.DoesNotThrow(() => SdkConfiguration.Validate());
        }

        [Test]
        public async Task Configure_DisplayNameFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(FilePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithDisplayNameMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "DisplayName is needed for ApplePay configuration (Parameter 'DisplayName')";
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, loggingConfiguration));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void Configure_AccessCodeFromApplePayIsMissing_ThrowsException()
        {
            //arrange
            //act
            var testFilePath = @"Configuration\MerchantSdkConfigurationWithAccessCodeMissingForApplePay.json";

            var expectedException = "AccessCode is needed for ApplePay configuration (Parameter 'AccessCode')";
            var actualException = Assert.Throws<ArgumentNullException>(() => SdkConfiguration.Configure(
                testFilePath, loggingConfiguration));

            //assert
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_RequestShaPhraseFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(FilePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithRequestShaPhraseMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "RequestShaPhrase is needed for ApplePay configuration (Parameter 'RequestShaPhrase')";
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, loggingConfiguration));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_ResponseShaPhraseFromApplePayIsMissing_ThrowsException()
        {
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(FilePath));

            var testFilePath = @"Configuration\MerchantSdkConfigurationWithResponseShaPhraseMissingForApplePay.json";
            //arrange
            //act
            //assert
            var expectedException = "ResponseShaPhrase is needed for ApplePay configuration (Parameter 'ResponseShaPhrase')";
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(testFilePath, loggingConfiguration));

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public async Task Configure_WhenCertificateIsSent_ReturnNoError()
        {
            //arrange
            //act
            //assert
            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(FilePath));
            Assert.DoesNotThrow(() =>
                SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration, new ApplePayConfiguration(certificate)));
        }

        [Test]
        public void Configure_WhenCertificateIsNull_ReturnError()
        {
            //arrange
            //act
            //assert
            const string expectedException = "SecurityCertificate is needed for ApplePay configuration (Parameter 'SecurityCertificate')";
            var actualException = Assert.Throws<ArgumentNullException>(() =>
                SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration, new ApplePayConfiguration(null)));

            Assert.That(actualException?.Message, Is.EqualTo(expectedException));
        }
    }
}
