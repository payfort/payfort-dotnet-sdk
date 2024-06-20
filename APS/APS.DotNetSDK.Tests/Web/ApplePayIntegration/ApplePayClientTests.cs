﻿using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Service;
using APS.DotNetSDK.Web.ApplePayIntegration;
using Microsoft.Extensions.Logging;
using Moq;
using static NUnit.Framework.Assert;

namespace APS.DotNetSDK.Tests.Web.ApplePayIntegration
{
    public class ApplePayClientTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly string _filePath = $"Web{Path.DirectorySeparatorChar.ToString()}ApplePayIntegration{Path.DirectorySeparatorChar.ToString()}Certificate.pem";
        private const string BaseUrl = "https://apple-pay-gateway-cert.apple.com";
        private const string RequestUri = "/paymentservices/paymentSession";

        private readonly Mock<IApiProxy> _apiProxyMock = new();

        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new();
        private readonly Mock<ILogger<ApplePayClient>> _loggerMock = new();


        [Test]
        public async Task RetrieveMerchantSession_ReturnsSession_InputIsValid()
        {
            //arrange
            var paymentSessionResponse = new PaymentSessionResponse()
            {
                MerchantIdentifier = "MerchantIdentifier",
                DisplayName = "DisplayName",
                Signature = "Signature",
                DomainName = "DomainName",
                EpochTimestamp = 123456456,
                ExpiresAt = 999999,
                MerchantSessionIdentifier = "MerchantSessionIdentifier",
                Nonce = "Nonce",
                OperationalAnalyticsIdentifier = "OperationalAnalyticsIdentifier",
                PspId = "PspId",
                Retries = 0
            };

            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            _apiProxyMock.Setup(x => x.PostAsync<PaymentSessionRequest, PaymentSessionResponse>(
                    It.IsAny<PaymentSessionRequest>(), BaseUrl, RequestUri))
                .ReturnsAsync(paymentSessionResponse).Verifiable();

            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactoryMock.Object,
                new ApplePayConfiguration(
                    certificate));

            var client = new ApplePayClient(_apiProxyMock.Object);

            //act
            var actualMerchantSession = await client.RetrieveMerchantSessionAsync($"{BaseUrl}{RequestUri}");

            //assert
            IsNotNull(actualMerchantSession);
            _apiProxyMock.Verify(x => x.PostAsync<PaymentSessionRequest, PaymentSessionResponse>(
                It.IsAny<PaymentSessionRequest>(), BaseUrl, RequestUri), Times.Once);
        }

        [Test]
        public async Task RetrieveMerchantSession_ThrowsException_UrlInputHasInvalidHttpScheme()
        {
            string urlWithInvalidHttpScheme = "http://test.com";

            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactoryMock.Object,
                new ApplePayConfiguration(
                    certificate));

            var client = new ApplePayClient(_apiProxyMock.Object);

            //act
            Assert.ThrowsAsync<ArgumentException>(async () => await client.RetrieveMerchantSessionAsync($"{urlWithInvalidHttpScheme}{RequestUri}"));
        }

        [Test]
        public async Task RetrieveMerchantSession_ThrowsException_UrlInputIsInvalid()
        {
            string urlWithInvalidHttpScheme = "bow-wow";

            var certificate = new X509Certificate2(await File.ReadAllBytesAsync(_filePath));
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactoryMock.Object,
                new ApplePayConfiguration(
                    certificate));

            var client = new ApplePayClient(_apiProxyMock.Object);

            //act
            Assert.ThrowsAsync<ArgumentException>(async () => await client.RetrieveMerchantSessionAsync($"{urlWithInvalidHttpScheme}{RequestUri}"));
        }

        [Test]
        public void RetrieveMerchantSession_ThrowsError_ApplePayIsNotConfigured()
        {
            string testFilePath = @"Configuration\MerchantSdkConfigurationWithApplePayMissing.json";
            SdkConfiguration.ClearConfiguration();
            //arrange
            SdkConfiguration.Configure(testFilePath, _loggerFactoryMock.Object);

            //act
            //assert
            Throws<SdkConfigurationException>(() =>
            {
                var applePayClient = new ApplePayClient();
            });
        }
    }
}