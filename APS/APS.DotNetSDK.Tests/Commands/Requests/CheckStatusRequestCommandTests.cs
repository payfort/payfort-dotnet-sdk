using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class CheckStatusRequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        [SetUp]
        public void Setup()
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration);
        }

        [Test]
        public void ValidateMandatoryProperty_AllMandatoryProperty_ReturnsNoError()
        {
            //arrange
            var objectTest = new CheckStatusRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        public void ValidateMandatoryProperty_MerchantReferencePropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new CheckStatusRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
            };

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        [TestCase("YES")]
        [TestCase("NO")]
        [TestCase("yes")]
        [TestCase("no")]
        public void ValidateMandatoryProperty_ReturnThirdPartyResponseCodes_ReturnNoError(string returnThirdPartyResponseCodes)
        {
            //arrange
            var objectTest = new CheckStatusRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                ReturnThirdPartyResponseCodes = returnThirdPartyResponseCodes
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        [TestCase("YESSS", "ReturnThirdPartyResponseCodes should be \"YES\" or \"NO\" (Parameter 'ReturnThirdPartyResponseCodes')")]
        [TestCase("NOOO", "ReturnThirdPartyResponseCodes should be \"YES\" or \"NO\" (Parameter 'ReturnThirdPartyResponseCodes')")]
        [TestCase("yesss", "ReturnThirdPartyResponseCodes should be \"YES\" or \"NO\" (Parameter 'ReturnThirdPartyResponseCodes')")]
        [TestCase("nooo", "ReturnThirdPartyResponseCodes should be \"YES\" or \"NO\" (Parameter 'ReturnThirdPartyResponseCodes')")]
        public void ValidateMandatoryProperty_ReturnThirdPartyResponseCodes_ReturnsError(string returnThirdPartyResponseCodes, string expectedException)
        {
            //arrange
            var objectTest = new CheckStatusRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                ReturnThirdPartyResponseCodes = returnThirdPartyResponseCodes
            };

            //act
            //assert
            var actualException = Assert.Throws<ArgumentException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
    }
}
