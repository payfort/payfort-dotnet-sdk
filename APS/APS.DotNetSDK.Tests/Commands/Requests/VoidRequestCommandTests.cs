using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class VoidRequestCommandTests
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
            var objectTest = new VoidRequestCommand()
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
            var objectTest = new VoidRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

    }
}
