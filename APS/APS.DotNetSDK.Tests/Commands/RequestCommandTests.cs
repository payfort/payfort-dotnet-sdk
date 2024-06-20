using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands
{
    public class RequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private Mock<ILoggerFactory> _loggerFactory = new Mock<ILoggerFactory>();
        [SetUp]
        public void Setup()
        {
            
            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                _loggerFactory.Object);
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
