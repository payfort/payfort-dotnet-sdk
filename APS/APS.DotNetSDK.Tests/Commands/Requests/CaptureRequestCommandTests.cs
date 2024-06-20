using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class CaptureRequestCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly Mock<ILoggerFactory> _loggerMock = new Mock<ILoggerFactory>();
        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerMock.Object);
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
