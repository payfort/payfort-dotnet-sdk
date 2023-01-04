using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Requests.ApplePay;
using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.Commands.Responses.ApplePay;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Maintenance;
using APS.DotNetSDK.Service;
using APS.DotNetSDK.Signature;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Cryptography.X509Certificates;
using ApplePayPaymentMethod = APS.DotNetSDK.Commands.Requests.ApplePay.ApplePayPaymentMethod;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Maintenance
{
    public class MaintenanceOperationsTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private const string FilePath = @"Maintenance\Certificate.pem";
        private readonly Mock<IApiProxy> _securedApiProxyMock = new();

        [SetUp]
        public void Setup()
        {

            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(
                FilePathMerchantConfiguration, loggingConfiguration,
                new ApplePayConfiguration(new X509Certificate2(FilePath)));
        }

        [Test]
        public async Task Authorize_InputIsValid_IsSuccessful()
        {
            //arrange
            var authorizeRequestCommand = new AuthorizeRequestCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var expectedAuthorizeResponseCommand = new AuthorizeResponseCommand
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                CustomerEmail = "test@email.com",
                Currency = "EUR",
                Language = "en",
                Signature = "36593ab4dbcd94254e12b7e30669cad458457617454236be58d3f4f4791d6f51"
            };

            //arrange, act, assert
            await TestMaintenanceOperation(authorizeRequestCommand,
                expectedAuthorizeResponseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.AuthorizeAsync((AuthorizeRequestCommand)command);
                });
        }

        [Test]
        public async Task AuthorizeWithApplePay_InputIsValid_IsSuccessful()
        {
            //arrange
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData
                    {
                        Data = "Data",
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            PublicKeyHash = "PublicKeyHash",
                            TransactionId = "TransactionId"
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = new ApplePayPaymentMethod
                    {
                        DisplayName = "DisplayName",
                        Network = "Network",
                        Type = "Type"
                    },
                    TransactionIdentifier = "TransactionIdentifier"
                }
            };

            var authorizeRequestCommand = new AuthorizeRequestCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var applePayAuthorizeRequestCommand = new ApplePayAuthorizeRequestCommand(authorizeRequestCommand, applePayRequestCommand);

            var expectedAuthorizeResponseCommand = new ApplePayAuthorizeResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                CustomerEmail = "test@email.com",
                Currency = "EUR",
                Language = "en",
                AppleData = applePayRequestCommand.Data.PaymentData.Data,
                Header = new ApplePayHeader
                {
                    EphemeralPublicKey = applePayRequestCommand.Data.PaymentData.Header.EphemeralPublicKey,
                    TransactionId = applePayRequestCommand.Data.PaymentData.Header.TransactionId,
                    PublicKeyHash = applePayRequestCommand.Data.PaymentData.Header.PublicKeyHash
                },
                PaymentMethod = new DotNetSDK.Commands.ApplePayPaymentMethod()
                {
                    DisplayName = applePayRequestCommand.Data.PaymentMethod.DisplayName,
                    Network = applePayRequestCommand.Data.PaymentMethod.Network,
                    Type = applePayRequestCommand.Data.PaymentMethod.Type,
                },
                AppleSignature = applePayRequestCommand.Data.PaymentData.Signature,
                Signature = "4b96893e506a6bd2a146e89e4622ff5f020f3aa6cc652a21c391109c62ad910d"
            };
            _securedApiProxyMock
                .Setup(x => x.PostAsync<ApplePayAuthorizeRequestCommand, ApplePayAuthorizeResponseCommand>(
                    It.IsAny<ApplePayAuthorizeRequestCommand>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedAuthorizeResponseCommand).Verifiable();

            //act
            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            var actualResponseCommand = await operation.AuthorizeAsync(authorizeRequestCommand, applePayRequestCommand);

            //assert
            //assert
            Assert.That(actualResponseCommand, Is.EqualTo(expectedAuthorizeResponseCommand));
            _securedApiProxyMock.Verify(x => x.PostAsync<ApplePayAuthorizeRequestCommand, ApplePayAuthorizeResponseCommand>(
                It.IsAny<ApplePayAuthorizeRequestCommand>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task PurchaseWithApplePay_InputIsValid_IsSuccessful()
        {
            //arrange
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData
                    {
                        Data = "Data",
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            PublicKeyHash = "PublicKeyHash",
                            TransactionId = "TransactionId"
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = new ApplePayPaymentMethod
                    {
                        DisplayName = "DisplayName",
                        Network = "Network",
                        Type = "Type"
                    },
                    TransactionIdentifier = "TransactionIdentifier"
                }
            };

            var authorizeRequestCommand = new PurchaseRequestCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var applePayAuthorizeRequestCommand = new ApplePayPurchaseRequestCommand(authorizeRequestCommand, applePayRequestCommand);

            var expectedAuthorizeResponseCommand = new ApplePayPurchaseResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                CustomerEmail = "test@email.com",
                Currency = "EUR",
                Language = "en",
                AppleData = applePayRequestCommand.Data.PaymentData.Data,
                Header = new ApplePayHeader
                {
                    EphemeralPublicKey = applePayRequestCommand.Data.PaymentData.Header.EphemeralPublicKey,
                    TransactionId = applePayRequestCommand.Data.PaymentData.Header.TransactionId,
                    PublicKeyHash = applePayRequestCommand.Data.PaymentData.Header.PublicKeyHash
                },
                PaymentMethod = new DotNetSDK.Commands.ApplePayPaymentMethod()
                {
                    DisplayName = applePayRequestCommand.Data.PaymentMethod.DisplayName,
                    Network = applePayRequestCommand.Data.PaymentMethod.Network,
                    Type = applePayRequestCommand.Data.PaymentMethod.Type,
                },
                AppleSignature = applePayRequestCommand.Data.PaymentData.Signature,
                Signature = "0efa227ac9d444e0d80c627cfdce512d8d2390e57c71069bb1af15e5ba55c908"
            };
            _securedApiProxyMock
                .Setup(x => x.PostAsync<ApplePayPurchaseRequestCommand, ApplePayPurchaseResponseCommand>(
                    It.IsAny<ApplePayPurchaseRequestCommand>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedAuthorizeResponseCommand).Verifiable();

            //act
            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            var actualResponseCommand = await operation.PurchaseAsync(authorizeRequestCommand, applePayRequestCommand);

            //assert
            //assert
            Assert.That(actualResponseCommand, Is.EqualTo(expectedAuthorizeResponseCommand));
            _securedApiProxyMock.Verify(x => x.PostAsync<ApplePayPurchaseRequestCommand, ApplePayPurchaseResponseCommand>(
                It.IsAny<ApplePayPurchaseRequestCommand>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Authorize_ResponseSignatureIsWrong_IsSuccessful()
        {
            //arrange
            var authorizeRequestCommand = new AuthorizeRequestCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var expectedAuthorizeResponseCommand = new AuthorizeResponseCommand
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                CustomerEmail = "test@email.com",
                Currency = "EUR",
                Language = "en",
                Signature = "wrong signature"
            };

            //arrange, act, assert
            Assert.ThrowsAsync<SignatureException>(async () =>
            {
                await TestMaintenanceOperation(authorizeRequestCommand,
                    expectedAuthorizeResponseCommand, async (command) =>
                    {
                        var operation =
                            new MaintenanceOperations(_securedApiProxyMock.Object);
                        return await operation.AuthorizeAsync((AuthorizeRequestCommand)command);
                    });
            });
        }

        [Test]
        public void Authorize_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var authorizeRequestCommand = new AuthorizeRequestCommand
            {
                MerchantReference = null,
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.AuthorizeAsync(authorizeRequestCommand));
        }

        [Test]
        public async Task Purchase_InputIsValid_IsSuccessful()
        {
            //arrange
            var requestCommand = new PurchaseRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var responseCommand = new PurchaseResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                CustomerEmail = "test@email.com",
                Currency = "EUR",
                Language = "en",
                Signature = "b55f5f87dd4d954ecd05dc19b6a29743605e952ecd61f16f581054a14fdf00bf"
            };

            //arrange, act, assert
            await TestMaintenanceOperation(requestCommand,
                responseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.PurchaseAsync((PurchaseRequestCommand)command);
                });
        }

        [Test]
        public void Purchase_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var requestCommand = new PurchaseRequestCommand()
            {
                MerchantReference = null,
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en",
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.PurchaseAsync(requestCommand));
        }

        [Test]
        public async Task Capture_InputIsValid_IsSuccessful()
        {
            //arrange
            var requestCommand = new CaptureRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "signature",
            };

            var responseCommand = new CaptureResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "4a86b5eff11c1eb68656fde994ec06314508fe1d716e2ca6a9caef1c190722c1"
            };

            //arrange, act, assert
            await TestMaintenanceOperation(requestCommand,
                responseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.CaptureAsync((CaptureRequestCommand)command);
                });
        }

        [Test]
        public void Capture_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var requestCommand = new CaptureRequestCommand()
            {
                MerchantReference = null,
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.CaptureAsync(requestCommand));
        }

        [Test]
        public async Task Void_InputIsValid_IsSuccessful()
        {
            //arrange
            var requestCommand = new VoidRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Language = "en",
                Signature = "signature",
            };

            var responseCommand = new VoidResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Language = "en",
                Signature = "5d0d102332e130408ce5a15172706399459fda4995ca71b0a2b797e5fc72acef"
            };

            //arrange, act, assert
            await TestMaintenanceOperation(requestCommand,
                responseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.VoidAsync((VoidRequestCommand)command);
                });
        }

        [Test]
        public void Void_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var requestCommand = new VoidRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Language = string.Empty,
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.VoidAsync(requestCommand));
        }

        [Test]
        public async Task Refund_InputIsValid_IsSuccessful()
        {
            //arrange
            var requestCommand = new RefundRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "signature",
            };

            var responseCommand = new RefundResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                Language = "en",
                Signature = "37c49fd9187a260548053ca921ecb1d5fc3678cb5f7ccd72d932bc332b67b80c",
            };

            //arrange, act, assert
            await TestMaintenanceOperation(requestCommand,
                responseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.RefundAsync((RefundRequestCommand)command);
                });
        }

        [Test]
        public void Refund_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var requestCommand = new RefundRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = string.Empty,
                Language = "en",
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.RefundAsync(requestCommand));
        }

        [Test]
        public async Task CheckStatus_InputIsValid_IsSuccessful()
        {
            //arrange
            var requestCommand = new CheckStatusRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Language = "en",
                Signature = "signature",
            };

            var responseCommand = new CheckStatusResponseCommand()
            {
                AccessCode = "AccessCode",
                MerchantIdentifier = "MerchantIdentifier",
                MerchantReference = "MerchantReference",
                Language = "en",
                Signature = "6da1974350f21eb0b9a9335bff02eafd05510314eba7d998838cee4262a3e235",
            };

            //arrange, act, assert
            await TestMaintenanceOperation(requestCommand,
                responseCommand, async (command) =>
                {
                    var operation =
                        new MaintenanceOperations(_securedApiProxyMock.Object);
                    return await operation.CheckStatusAsync((CheckStatusRequestCommand)command);
                });
        }

        [Test]
        public void CheckStatus_InputIsNotValid_ExceptionIsThrown()
        {
            //arrange
            var requestCommand = new CheckStatusRequestCommand()
            {
                MerchantReference = string.Empty,
                Language = "en",
                Signature = "signature",
            };

            var operation =
                new MaintenanceOperations(_securedApiProxyMock.Object);

            //act, assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await operation.CheckStatusAsync(requestCommand));
        }
        
        private async Task TestMaintenanceOperation<TRequest, TResponse>(TRequest requestCommand,
            TResponse expectedResponseCommand, Func<object, Task<TResponse>> func)
        {
            //arrange
            _securedApiProxyMock
                .Setup(x => x.PostAsync<TRequest, TResponse>(
                    requestCommand, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponseCommand).Verifiable();

            //act
            var actualResponseCommand = await func(requestCommand!);

            //assert
            Assert.That(actualResponseCommand, Is.EqualTo(expectedResponseCommand));
            _securedApiProxyMock.Verify(x => x.PostAsync<TRequest, TResponse>(
                requestCommand, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}