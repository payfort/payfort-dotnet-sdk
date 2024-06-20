using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Requests.ApplePay;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Commands.Requests.ApplePay
{
    public class ApplePayPurchaseCommandTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private SdkConfigurationDto _sdkConfigurationDto;
        private Mock<ILoggerFactory> _loggerFactory = new Mock<ILoggerFactory>();

        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(FilePathMerchantConfiguration, _loggerFactory.Object);
            _sdkConfigurationDto = SdkConfiguration.GetAccount("MainAccount");
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ReturnsCorrectOutput_InputIsValid()
        {
            var requestCommand = new ApplePayRequestCommand
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
            var purchaseRequestCommand = new PurchaseRequestCommand()
            {
                MerchantReference = "MerchantReference",
                Amount = 2.0,
                Currency = "EUR",
                CustomerEmail = "test@email.com",
                Language = "en"
            };

            var applePayPurchaseRequestCommand =
                new ApplePayPurchaseRequestCommand(purchaseRequestCommand, requestCommand, _sdkConfigurationDto);
            Assert.Multiple(() =>
            {
                Assert.That(applePayPurchaseRequestCommand.AppleData,
                            Is.EqualTo(requestCommand.Data.PaymentData.Data));
                Assert.That(applePayPurchaseRequestCommand.Header.TransactionId,
                    Is.EqualTo(requestCommand.Data.PaymentData.Header.TransactionId));
                Assert.That(applePayPurchaseRequestCommand.Header.PublicKeyHash,
                    Is.EqualTo(requestCommand.Data.PaymentData.Header.PublicKeyHash));
                Assert.That(applePayPurchaseRequestCommand.Header.EphemeralPublicKey,
                    Is.EqualTo(requestCommand.Data.PaymentData.Header.EphemeralPublicKey));
                Assert.That(applePayPurchaseRequestCommand.AppleSignature,
                    Is.EqualTo(requestCommand.Data.PaymentData.Signature));
                Assert.That(applePayPurchaseRequestCommand.PaymentMethod.DisplayName,
                    Is.EqualTo(requestCommand.Data.PaymentMethod.DisplayName));
                Assert.That(applePayPurchaseRequestCommand.PaymentMethod.Network,
                    Is.EqualTo(requestCommand.Data.PaymentMethod.Network));
                Assert.That(applePayPurchaseRequestCommand.PaymentMethod.Type,
                    Is.EqualTo(requestCommand.Data.PaymentMethod.Type));
                Assert.That(applePayPurchaseRequestCommand.MerchantReference,
                    Is.EqualTo(purchaseRequestCommand.MerchantReference));
                Assert.That(applePayPurchaseRequestCommand.Amount,
                    Is.EqualTo(purchaseRequestCommand.Amount));
                Assert.That(applePayPurchaseRequestCommand.Currency,
                    Is.EqualTo(purchaseRequestCommand.Currency));
                Assert.That(applePayPurchaseRequestCommand.CustomerEmail,
                    Is.EqualTo(purchaseRequestCommand.CustomerEmail));
                Assert.That(applePayPurchaseRequestCommand.Language,
                    Is.EqualTo(purchaseRequestCommand.Language));
                Assert.That(applePayPurchaseRequestCommand.AccessCode,
                    Is.EqualTo("TestAccessCode"));
                Assert.That(applePayPurchaseRequestCommand.MerchantIdentifier,
                    Is.EqualTo("TestMerchantIdentifier"));
            });
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_DataIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = null
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_PaymentDataDataIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = null
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_HeaderIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = null
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_EphemeralPublicKeyIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = null,
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash"
                        }
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_PublicKeyHashIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = null
                        }
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_TransactionIdIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = null,
                            PublicKeyHash = "PublicKeyHash"
                        }
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_SignatureIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = null
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_VersionIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = "Signature",
                        Version = null
                    }
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_PaymentMethodIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = null

                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_DisplayNameIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = new ApplePayPaymentMethod
                    {
                        DisplayName = null,
                        Network = "Network",
                        Type = "Type"
                    }

                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_NetworkIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = new ApplePayPaymentMethod
                    {
                        DisplayName = "DisplayName",
                        Network = null,
                        Type = "Type"

                    }

                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayPurchaseRequestCommand_ThrowException_TypeIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = new ApplePayPaymentData()
                    {
                        Header = new ApplePayPaymentHeader
                        {
                            EphemeralPublicKey = "EphemeralPublicKey",
                            TransactionId = "TransactionId",
                            PublicKeyHash = "PublicKeyHash",
                        },
                        Signature = "Signature",
                        Version = "Version"
                    },
                    PaymentMethod = new ApplePayPaymentMethod
                    {
                        DisplayName = "DisplayName",
                        Network = "Network",
                        Type = null

                    }

                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayPurchaseRequestCommand(new PurchaseRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }
    }
}
