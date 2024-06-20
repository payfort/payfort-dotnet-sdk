using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Service;
using Moq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace APS.DotNetSDK.Tests.Service
{
    public class ApiProxyTests
    {
        private readonly Mock<IHttpClientWrapper> _httpClientWrapperMock = new();
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
        public async Task PostAsync_InputIsValid_IsSuccessful()
        {
            var requestCommand = new CheckStatusRequestCommand
            {
                AccessCode = "TestAccessCode",
                MerchantIdentifier = "TestMerchantIdentifier",
                MerchantReference = "MerchantReference",
                Language = "en"
            };

            var responseCommand = new CheckStatusResponseCommand
            {
                MerchantReference = "MerchantReference",
                Language = "en",
                Status = "Success",
                Signature = "Signature"
            };

            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent(JsonSerializer.Serialize(responseCommand));


            //arrange
            _httpClientWrapperMock.Setup(x =>
                    x.PostAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(responseMessage);

            var proxy = new ApiProxy(_httpClientWrapperMock.Object);

            //act
            var expectedResponse =
                await proxy.PostAsync<CheckStatusRequestCommand, CheckStatusResponseCommand>(requestCommand,
                    "http://test/com", "/test");

            Assert.That(responseCommand.AccessCode, Is.EqualTo(expectedResponse.AccessCode));
            Assert.That(responseCommand.MerchantIdentifier, Is.EqualTo(expectedResponse.MerchantIdentifier));
            Assert.That(responseCommand.MerchantReference, Is.EqualTo(expectedResponse.MerchantReference));
            Assert.That(responseCommand.Language, Is.EqualTo(expectedResponse.Language));
            Assert.That(responseCommand.Status, Is.EqualTo("Success"));
        }
    }
}
