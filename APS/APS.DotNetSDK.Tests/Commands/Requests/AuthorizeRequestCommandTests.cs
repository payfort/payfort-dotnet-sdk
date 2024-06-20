using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class AuthorizeRequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private SdkConfigurationDto _sdkConfigurationDto;
        private  Mock<ILoggerFactory> _loggerMock = new Mock<ILoggerFactory>();

        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerMock.Object);
            _sdkConfigurationDto = SdkConfiguration.GetAccount("MainAccount");
        }
        [Test]
        public void ValidateMandatoryProperty_AllMandatoryProperty_ReturnsNoError()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestCurrencyEmail",
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        public void ValidateMandatoryProperty_MerchantReferencePropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestCurrencyEmail",
            };

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ValidateMandatoryProperty_CurrencyPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Amount = 24.3,
                CustomerEmail = "TestCurrencyEmail",
            };

            //act
            //assert
            var expectedException = "Currency is mandatory (Parameter 'Currency')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ValidateMandatoryProperty_AmountPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                CustomerEmail = "TestCurrencyEmail"
            };

            //act
            //assert
            var expectedException = "Amount is mandatory (Parameter 'Amount')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ValidateMandatoryProperty_CustomerEmailPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3,
            };

            //act
            //assert
            var expectedException = "CustomerEmail is mandatory (Parameter 'CustomerEmail')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        [TestCase("YES")]
        [TestCase("NO")]
        [TestCase("yes")]
        [TestCase("no")]
        public void ValidateMandatoryProperty_RememberMeProperty_ReturnNoError(string rememberMe)
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestCurrencyEmail",
                RememberMe = rememberMe
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        [TestCase("YESSS", "RememberMe should be \"YES\" or \"NO\" (Parameter 'RememberMe')")]
        [TestCase("NOOO", "RememberMe should be \"YES\" or \"NO\" (Parameter 'RememberMe')")]
        [TestCase("yesss", "RememberMe should be \"YES\" or \"NO\" (Parameter 'RememberMe')")]
        [TestCase("nooo", "RememberMe should be \"YES\" or \"NO\" (Parameter 'RememberMe')")]
        public void ValidateMandatoryProperty_RememberMeProperty_ReturnsError(string rememberMe, string expectedException)
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestCurrencyEmail",
                RememberMe = rememberMe
            };

            //act
            //assert
            var actualException = Assert.Throws<ArgumentException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ToAnonymizedJson_ReturnsAnonymizedJson()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand()
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestCustomerEmail",
            };

            //act
            var expectedResult = "{\"command\":\"AUTHORIZATION\",\"amount\":24.3,\"currency\":\"TestCurrency\",\"customer_email\":\"***\"," +
                "\"app_programming\":\".NET\",\"app_plugin\":\".dotNETSDK\",\"app_plugin_version\":\"v2.1.0\",\"app_ver\":\"1.0.0.0\"," +
                "\"app_framework\":\".NET\",\"access_code\":\"TestAccessCode\",\"merchant_identifier\":\"TestMerchantIdentifier\",\"merchant_reference\":\"TestMerchantReference\"," +
                "\"language\":\"testlanguage\",\"signature\":\"TestSignature\"}";

            //assert
            var actualResult = objectTest.ToAnonymizedJson();

           Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
