using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Commands
{
    public class RequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        [SetUp]
        public void Setup()
        {

            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                loggingConfiguration);
        }

        [Test]
        public void ValidateMandatoryProperty_AllMandatoryProperties_ReturnsNoError()
        {
            //arrange
            var objectTest = new TestRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        public void ValidateMandatoryProperty_AccessCodePropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new TestRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());

        }

        [Test]
        public void ValidateMandatoryProperty_MerchantIdentifierPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new TestRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        public void ValidateMandatoryProperty_LanguagePropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new TestRequestCommand()
            {
                Signature = "TestSignature"
            };

            //act
            //assert
            string expectedException = "Language is mandatory (Parameter 'Language')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }


        [Test]
        [TestCase("EN")]
        [TestCase("en")]
        public void ValidateMandatoryProperty_LanguageProperty_ReturnNoError(string language)
        {
            //arrange
            var objectTest = new TestRequestCommand()
            {
                Language = language,
                Signature = "TestSignature"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
            Assert.That(objectTest.Language, Is.EqualTo(language.ToLower()));
        }
    }
}
