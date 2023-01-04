using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Service;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Web.Installments;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Web.Installments
{
    public class InstallmentsProviderTests
    {
        private readonly Mock<IApiProxy> _securedApiProxyMock = new();
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
        public async Task GetInstallments_InputIsValid_IsSuccessful()
        {
            //arrange
            var getInstallmentsRequestCommand = new GetInstallmentsRequestCommand
            {
                Language = "en",
                Signature = "signature",
            };

            var expectedGetInstallmentsResponseCommand = new GetInstallmentsResponseCommand
            {
                Language = "en",
                Signature = "bad8efe66b9dad9d7af272d602372c51c93a0ec5a087d446cd3dbb92a941047c"
            };

            //arrange, act, assert
            await TestGetInstallments(getInstallmentsRequestCommand,
                expectedGetInstallmentsResponseCommand, async (command) =>
                {
                    var installments =
                            new InstallmentsProvider(_securedApiProxyMock.Object);
                    return await installments.GetInstallmentsPlansAsync((GetInstallmentsRequestCommand)command);
                });
        }

        [Test]
        public async Task GetInstallments_InputIsValidAndHasOptionalParameters_IsSuccessful()
        {
            //arrange
            var getInstallmentsRequestCommand = new GetInstallmentsRequestCommand
            {
                Amount = 243000,
                Currency="AED",
                Language = "en",
                Signature = "signature",
                IssuerCode  ="TestIssuerCode"
            };

            var expectedGetInstallmentsResponseCommand = new GetInstallmentsResponseCommand
            {
                Amount = 243000,
                Currency = "AED",
                Language = "en",
                Signature = "cbc96c9c2e8d28467429584b5beabdb52537afcfe140d716603ca551f436d154",
                IssuerCode = "TestIssuerCode"
            };

            //arrange, act, assert
            await TestGetInstallments(getInstallmentsRequestCommand,
                expectedGetInstallmentsResponseCommand, async (command) =>
                {
                    var installments =
                            new InstallmentsProvider(_securedApiProxyMock.Object);
                    return await installments.GetInstallmentsPlansAsync((GetInstallmentsRequestCommand)command);
                });
        }

        [Test]
        public void GetInstallments_ResponseSignatureIsWrong_IsSuccessful()
        {
            //arrange
            var getInstallmentsRequestCommand = new GetInstallmentsRequestCommand
            {
                Language = "en",
                Signature = "signature",
            };

            var expectedGetInstallmentsResponseCommand = new GetInstallmentsResponseCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "wrong signature"
            };

            //arrange, act, assert
            Assert.ThrowsAsync<SignatureException>(async () =>
            {
                await TestGetInstallments(getInstallmentsRequestCommand,
                    expectedGetInstallmentsResponseCommand, async (command) =>
                    {
                        var installments =
                            new InstallmentsProvider(_securedApiProxyMock.Object);
                        return await installments.GetInstallmentsPlansAsync((GetInstallmentsRequestCommand)command);
                    });
            });
        }

        private async Task TestGetInstallments(GetInstallmentsRequestCommand requestCommand,
            GetInstallmentsResponseCommand expectedResponseCommand, Func<object, Task<GetInstallmentsResponseCommand>> func)
        {
            //arrange
            _securedApiProxyMock
                .Setup(x => x.PostAsync<GetInstallmentsRequestCommand, GetInstallmentsResponseCommand>(
                    requestCommand, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponseCommand).Verifiable();

            //act
            var actualResponseCommand = await func(requestCommand!);

            //assert
            Assert.That(actualResponseCommand, Is.EqualTo(expectedResponseCommand));
            _securedApiProxyMock.Verify(x => x.PostAsync<GetInstallmentsRequestCommand, GetInstallmentsResponseCommand>(
                requestCommand, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
