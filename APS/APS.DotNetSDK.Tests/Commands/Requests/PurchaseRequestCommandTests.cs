using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class PurchaseRequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        [SetUp]
        public void Setup()
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(FilePathMerchantConfiguration, loggingConfiguration);
        }

        [Test]
        public void ValidateMandatoryProperty_AllMandatoryProperties_ReturnsNoError()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand()
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
            var objectTest = new PurchaseRequestCommand()
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
        public void ValidateMandatoryProperty_CurrencyCodePropertyIsMissin_ReturnsError()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand()
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
            var objectTest = new PurchaseRequestCommand()
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
            var objectTest = new PurchaseRequestCommand()
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
            var objectTest = new PurchaseRequestCommand()
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
            var objectTest = new PurchaseRequestCommand()
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
    }
}
