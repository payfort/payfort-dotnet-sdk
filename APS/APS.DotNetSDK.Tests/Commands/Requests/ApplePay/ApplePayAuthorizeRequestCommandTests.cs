using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Commands.Requests.ApplePay;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using Moq;


namespace APS.DotNetSDK.Tests.Commands.Requests.ApplePay
{
    public class ApplePayAuthorizeRequestCommandTests
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
        public void ApplePayAuthorizeRequestCommand_ReturnsCorrectOutput_InputIsValid()
        {
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
                Language = "en"
            };

            var applePayAuthorizeRequestCommand =
                new ApplePayAuthorizeRequestCommand(authorizeRequestCommand, applePayRequestCommand, _sdkConfigurationDto);
            Assert.Multiple(() =>
            {
                Assert.That(applePayAuthorizeRequestCommand.AppleData,
                            Is.EqualTo(applePayRequestCommand.Data.PaymentData.Data));
                Assert.That(applePayAuthorizeRequestCommand.Header.TransactionId,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentData.Header.TransactionId));
                Assert.That(applePayAuthorizeRequestCommand.Header.PublicKeyHash,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentData.Header.PublicKeyHash));
                Assert.That(applePayAuthorizeRequestCommand.Header.EphemeralPublicKey,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentData.Header.EphemeralPublicKey));
                Assert.That(applePayAuthorizeRequestCommand.AppleSignature,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentData.Signature));
                Assert.That(applePayAuthorizeRequestCommand.PaymentMethod.DisplayName,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentMethod.DisplayName));
                Assert.That(applePayAuthorizeRequestCommand.PaymentMethod.Network,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentMethod.Network));
                Assert.That(applePayAuthorizeRequestCommand.PaymentMethod.Type,
                    Is.EqualTo(applePayRequestCommand.Data.PaymentMethod.Type));
                Assert.That(applePayAuthorizeRequestCommand.MerchantReference,
                    Is.EqualTo(authorizeRequestCommand.MerchantReference));
                Assert.That(applePayAuthorizeRequestCommand.Amount,
                    Is.EqualTo(authorizeRequestCommand.Amount));
                Assert.That(applePayAuthorizeRequestCommand.Currency,
                    Is.EqualTo(authorizeRequestCommand.Currency));
                Assert.That(applePayAuthorizeRequestCommand.CustomerEmail,
                    Is.EqualTo(authorizeRequestCommand.CustomerEmail));
                Assert.That(applePayAuthorizeRequestCommand.Language,
                    Is.EqualTo(authorizeRequestCommand.Language));
                Assert.That(applePayAuthorizeRequestCommand.AccessCode,
                    Is.EqualTo("TestAccessCode"));
                Assert.That(applePayAuthorizeRequestCommand.MerchantIdentifier,
                    Is.EqualTo("TestMerchantIdentifier"));
            });
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_DataIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = null
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_PaymentDataDataIsNull()
        {
            var applePayRequestCommand = new ApplePayRequestCommand
            {
                Data = new ApplePayData
                {
                    PaymentData = null
                }
            };

            Assert.Throws<ArgumentNullException>(() =>
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_HeaderIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_EphemeralPublicKeyIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_PublicKeyHashIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_TransactionIdIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_SignatureIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_VersionIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_PaymentMethodIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_DisplayNameIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_NetworkIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));

        }

        [Test]
        public void ApplePayAuthorizeRequestCommand_ThrowException_TypeIsNull()
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
                new ApplePayAuthorizeRequestCommand(new AuthorizeRequestCommand(), applePayRequestCommand, _sdkConfigurationDto));
        }
    }
}