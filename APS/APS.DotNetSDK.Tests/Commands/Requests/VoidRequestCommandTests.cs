using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands.Requests
{
    public class VoidRequestCommandTests
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
