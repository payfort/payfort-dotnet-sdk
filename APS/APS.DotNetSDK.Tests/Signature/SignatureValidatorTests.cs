using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Tests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Signature
{
    public class SignatureValidatorTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private const string Phrase = "PASS";

        [SetUp]
        public void Setup()
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                loggingConfiguration);
        }

        [Test]
        [TestCase(true,"6e35631fc0a493adfa062eb173cf6e37751f6e12dfd3eea9b9eec656559920d2", ShaType.Sha256)]
        [TestCase(true,"743f3ea5d8d4c67f07e936512ed4fadfd7236fe39e4d8ec0cf779736155d2edf97e769536c9a074609919b3b62330b43c5a41540b4e48809bdced50904d9fa0a", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfProperties_ReturnsCorrectSignature(bool expectedResult, string signature, ShaType shaType)
        {
            //arrange
            var objectTest = new TestRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };

            var serviceSignatureProvider = new SignatureProvider();
            var service = new SignatureValidator(serviceSignatureProvider);

            //act
            var actualResult = service.ValidateSignature(objectTest, Phrase, shaType, signature);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

    }
}
