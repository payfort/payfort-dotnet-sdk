using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Tests.Signature.Models;
using APS.Signature;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Signature
{
    public class SignatureProviderTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private SdkConfigurationDto _sdkConfigurationDto;
        private readonly Mock<ILoggerFactory> _loggerMock = new Mock<ILoggerFactory>();
        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                _loggerMock.Object);
            _sdkConfigurationDto = SdkConfiguration.GetAccount("MainAccount");

        }

        [Test]
        [TestCase("6e35631fc0a493adfa062eb173cf6e37751f6e12dfd3eea9b9eec656559920d2", ShaType.Sha256)]
        [TestCase("743f3ea5d8d4c67f07e936512ed4fadfd7236fe39e4d8ec0cf779736155d2edf97e769536c9a074609919b3b62330b43c5a41540b4e48809bdced50904d9fa0a", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfProperties_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new OneLevelPropertyRequestCommand()
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("68c3928525ec3914129b71d167061b6044d05727367ad7a88f3ac8ad173feea9", ShaType.Sha256)]
        [TestCase("001d164c9d5c7ce83f64cb0d94cccc12a64825ef013e9799813a7ad0a41f9110319e192fda974a482bec9b0d4495d7d0bfe1a767ec40f6918307d495e652faaf", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfPropertiesAndOneNullProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new OneLevelPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = null,
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("68c3928525ec3914129b71d167061b6044d05727367ad7a88f3ac8ad173feea9", ShaType.Sha256)]
        [TestCase("001d164c9d5c7ce83f64cb0d94cccc12a64825ef013e9799813a7ad0a41f9110319e192fda974a482bec9b0d4495d7d0bfe1a767ec40f6918307d495e652faaf", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfPropertiesAndOneEmptyProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new OneLevelPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("53c5aa02e0cdf14d59cc2eef557b23bffa33621f5f6faf14d3c215f8d4db1954", ShaType.Sha256)]
        [TestCase("e60a3600f0230728f2bf8907a0989b8b95aef7c60e7aa563519bf54fb6a371fa8c5a85bee97f57428e177d48037e8ba07b488b658c43323a744f11c9e3ffc927", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfProperties_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new Payment
                {
                    PaymentMethod = "TestPaymentMethod",
                    CardType = "TestCardType",
                    SecurityCode = 123
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("d5daf610089a70484d730f784981842638ce41baea2744496215761894737ae4", ShaType.Sha256)]
        [TestCase("1846f9c7de1192ba5a8bf6bdabc6b7c768659726e040b3504f34fdbe5c8629cd038f4ec18e80035537140706e51375f5cbcbf60a1ad4e6df68b71a297680d4a4", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneEmptyProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new Payment
                {
                    PaymentMethod = "",
                    CardType = "TestCardType"
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("8605fcee68061bb5921dd96535fc87a850502822a9881da6de4b194055a36e0a", ShaType.Sha256)]
        [TestCase("5e14da732f6efb8ca6712a1e52bc6c11d13f7a10881ef27d83e48cb73b37625fe2543c48136eb51c062aa1495f19e1ce302dfb559f1a38b5e5e305ed686b94fe", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneIntProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new Payment
                {
                    PaymentMethod = "",
                    CardType = "TestCardType",
                    SecurityCode = 345
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("d5daf610089a70484d730f784981842638ce41baea2744496215761894737ae4", ShaType.Sha256)]
        [TestCase("1846f9c7de1192ba5a8bf6bdabc6b7c768659726e040b3504f34fdbe5c8629cd038f4ec18e80035537140706e51375f5cbcbf60a1ad4e6df68b71a297680d4a4", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneNullProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new Payment
                {
                    PaymentMethod = null,
                    CardType = "TestCardType"
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("7c0ec9171ed177d07039a3cb4eb4a63608189ef3ad0ae2abb755476aede1c062", ShaType.Sha256)]
        [TestCase("81359ca20e847587537aa263e269795c9af84e9421874561c9aeaf18d63376ca47664a036a9eecb0c3a8f2c4dc1ca4c1d0eaa53c25306b08ec2fc71b874fc653", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollection_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new List<Payment>
                {
                    new Payment
                    {
                        PaymentMethod = "TestPaymentMethod1",
                        CardType = "TestCardType1"
                    },
                    new Payment
                    {
                        PaymentMethod = "TestPaymentMethod2",
                        CardType = "TestCardType2"
                    }
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase("94ea2c089166b2969313c63b3f15bb6a3e223bb5b92c0e54e948e1bc60b83809", ShaType.Sha256)]
        [TestCase("82f0f046bab28fa41267ec88cf7c983d1f13201616042c1948e43a73e9f2e261340f223b49a80701081cb53d5d8ebabe1fcb381eb71c0dd6208ec738ee12c95a", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollectionWithOneEmptyProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new List<Payment>
                {
                    new Payment
                    {
                        PaymentMethod = "",
                        CardType = "TestCardType1"
                    },
                    new Payment
                    {
                        PaymentMethod = "TestPaymentMethod2",
                        CardType = "TestCardType2"
                    }
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("94ea2c089166b2969313c63b3f15bb6a3e223bb5b92c0e54e948e1bc60b83809", ShaType.Sha256)]
        [TestCase("82f0f046bab28fa41267ec88cf7c983d1f13201616042c1948e43a73e9f2e261340f223b49a80701081cb53d5d8ebabe1fcb381eb71c0dd6208ec738ee12c95a", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollectionWithOneNullProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new List<Payment>
                {
                    new Payment
                    {
                        PaymentMethod = null,
                        CardType = "TestCardType1"
                    },
                    new Payment
                    {
                        PaymentMethod = "TestPaymentMethod2",
                        CardType = "TestCardType2"
                    }
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [Ignore("Now supports more that two levels of json")]
        [TestCase("Only two levels of reference types are allowed for signature calculation.", ShaType.Sha256)]
        [TestCase("Only two levels of reference types are allowed for signature calculation.", ShaType.Sha512)]
        public void GetSignature_InputHasThreeLevelsOfPropertiesWithCollection_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new ThreeLevelsPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                CustomerDetail = new Customer()
                {
                    CustomerEmail = "TestCustomerEmail",
                    BillingDetails = new CustomerBillingDetails()
                    {
                        StreetName = "TestStreetName",
                        BillingCountry = new BillingCountry()
                        {
                            Country = "TestCountry",
                            CountryCode = 45
                        }
                    }
                }
            };

            var service = new SignatureProvider();

            //act
            //assert
            var actualException = Assert.Throws<SignatureException>(() => service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType));
            Assert.That(actualException.Message, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("201124db3690c475592d044034fbacebc387cb4d09f459c591f25ad1cd8ca160", ShaType.Sha256)]
        [TestCase("a7726646fe6763c5f29db9c697a148e4569d2cdbd3be58c39c1e4dc01c3a6f3834e2b042f1e1fe99e644eae9ae5e66abc5d1abb1afefb7d2adf92d8fe8569bf1", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfPropertiesOrdered_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new OneLevelPropertyRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature",
                ZBank = "Banca Transilvania",
                XAuthenticationCode = "1234"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("ff1172905d076377006b8581f2f55261d8545c9e81c678081f385ef23be40187", ShaType.Sha256)]
        [TestCase("6457ad9811a35336b548da51e7ecb6d6c52d3086d3dde5dcd50d7fad89827a0a1e29fc1c9a20be7928c3d63732706118949059092037fd53defb080cb42cc083", ShaType.Sha512)]
        public void GetSignature_AuthorizeRequestCommand_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);
            
            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("be88e74d52cbcdf90612c2b560a9a68444760e070d829fb0df93ef68caf339a9", ShaType.Sha256)]
        [TestCase("b96c16eecb81aec38fd931b011ac009f946b5ccf5ec23f97a26bbbe812b60dfc8b06dcf88dbbe08439f0bc68de4e2223e99d4830d0ede8c46c9b9a70d8dfc747", ShaType.Sha512)]
        public void GetSignature_PurchaseRequestCommand_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                AccessCode = _sdkConfigurationDto.AccessCode,
                MerchantIdentifier = _sdkConfigurationDto.MerchantIdentifier,
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, _sdkConfigurationDto.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
