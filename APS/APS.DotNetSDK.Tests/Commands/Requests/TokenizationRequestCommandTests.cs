using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class TokenizationRequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private SdkConfigurationDto _sdkConfigurationDto;
        private readonly Mock<ILoggerFactory> _loggerMock = new Mock<ILoggerFactory>();
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
            var objectTest = new TokenizationRequestCommand()
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
            var objectTest = new TokenizationRequestCommand()
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

        [Test]
        [TestCase("YES")]
        [TestCase("NO")]
        [TestCase("yes")]
        [TestCase("no")]
        public void ValidateMandatoryProperty_RememberMeProperty_ReturnNoError(string rememberMe)
        {
            //arrange
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
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
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
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
            var objectTest = new TokenizationRequestCommand()
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                CardHolderName = "TestCardHolderName",
                ExpiryDate = "TestExpiryDate",
                CardNumber = "TestCardNumber",
                SecurityCode = "TestCardSecurityCode"
            };

            //act
            var expectedResult = "{\"service_command\":\"TOKENIZATION\",\"expiry_date\":\"***\",\"card_number\":\"***\"," +
                "\"card_security_code\":\"***\",\"card_holder_name\":\"***\",\"access_code\":\"TestAccessCode\",\"merchant_identifier\":\"TestMerchantIdentifier\"," +
                "\"merchant_reference\":\"TestMerchantReference\",\"language\":\"testlanguage\",\"signature\":\"TestSignature\"}";

            //assert
            var actualResult = objectTest.ToAnonymizedJson();

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        #region Installments

        [Test]
        public void ValidateMandatoryPropertyInstallments_AllMandatoryProperty_ReturnsNoError()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Installments = "Standalone",
                Amount = 24.3,
                Currency = "AED"
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryPropertiesInstallments());
        }

        [Test]
        public void ValidateMandatoryProperty_InstallmentsPropertyIsDifferent_ReturnsError()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Installments = "TestInstallments",
                Amount = 24.3,
                Currency = "AED"
            };

            //act
            //assert
            var expectedException = "Installments should be \"STANDALONE\" (Parameter 'Installments')";
            var actualException = Assert.Throws<ArgumentException>(() => objectTest.ValidateMandatoryProperties());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ValidateMandatoryPropertyInstallments_AmountPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Installments = "TestInstallments",
                Amount = 0,
                Currency = "AED"
            };

            //act
            //assert
            var expectedException = "Amount is mandatory for installments (Parameter 'Amount')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryPropertiesInstallments());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void ValidateMandatoryPropertyInstallments_CurrencyPropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Installments = "TestInstallments",
                Amount = 24.3
            };

            //act
            //assert
            var expectedException = "Currency is mandatory for installments (Parameter 'Currency')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryPropertiesInstallments());
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        #endregion
    }
}
