using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class CaptureRequestCommandTests
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
            var objectTest = new CaptureRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency",
                Amount = 24.3
            };

            //act
            //assert
            Assert.DoesNotThrow(() => objectTest.ValidateMandatoryProperties());
        }

        [Test]
        public void ValidateMandatoryProperty_MerchantReferencePropertyIsMissing_ReturnsError()
        {
            //arrange
            var objectTest = new CaptureRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                Currency = "TestCurrency",
                Amount = 24.3
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
            var objectTest = new CaptureRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Amount = 24.3
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
            var objectTest = new CaptureRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                MerchantReference = "TestMerchantReference",
                Currency = "TestCurrency"
            };

            //act
            //assert
            var expectedException = "Amount is mandatory (Parameter 'Amount')";
            var actualException = Assert.Throws<ArgumentNullException>(() => objectTest.ValidateMandatoryProperties());

            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
    }
}
