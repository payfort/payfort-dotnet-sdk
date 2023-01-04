using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Exceptions;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Tests.Signature.Models;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Signature
{
    public class SignatureProviderTests
    {
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
        [TestCase("6e35631fc0a493adfa062eb173cf6e37751f6e12dfd3eea9b9eec656559920d2", ShaType.Sha256)]
        [TestCase("743f3ea5d8d4c67f07e936512ed4fadfd7236fe39e4d8ec0cf779736155d2edf97e769536c9a074609919b3b62330b43c5a41540b4e48809bdced50904d9fa0a", ShaType.Sha512)]
        public void GetSignature_InputHasOneLevelOfProperties_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new OneLevelPropertyRequestCommand()
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();
            
            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

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
                Language = null,
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

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
                Language = "",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("00c75fb04b120e37f61f708f9a4e713bcd1212829cd54c979e2adcf4722d1443", ShaType.Sha256)]
        [TestCase("d3c85701c355ff88665b5d06fcab6066f4c62788bc4041faa20d6353c262e976f10fe2bd4554bc9e10318892b3c49211a70c68b8b8b1941180c0dcff053d6d1f", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfProperties_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                PaymentType = new Payment
                {
                    PaymentMethod = "TestPaymentMethod",
                    CardType = "TestCardType"
                }
            };

            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("59cc26e90b9208f95853d84db92dcbd8be659f296dacedf9cad0e3727d3260b3", ShaType.Sha256)]
        [TestCase("12cc65778db4f2a7184699e5e93bf4ee0d0bce998a729e50e67d36cf543cd9c32d3c5c6f3adfd7cc0e360d64acf97d91076edda588e415928b34dd01a574971e", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneEmptyProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("f977b51d87dfa6e608f51585fa683a8410cec73f3ce1f4dcf77c513731363c1d", ShaType.Sha256)]
        [TestCase("90e17b7b6ed041632a333f179b93a9bb3feef4bd9cf97b3e2a612a707666c19a6cd7b0c29ea8170c39edfa36a87c1fd3b5861906f9a0c88814ded40387c772e6", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneIntProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("59cc26e90b9208f95853d84db92dcbd8be659f296dacedf9cad0e3727d3260b3", ShaType.Sha256)]
        [TestCase("12cc65778db4f2a7184699e5e93bf4ee0d0bce998a729e50e67d36cf543cd9c32d3c5c6f3adfd7cc0e360d64acf97d91076edda588e415928b34dd01a574971e", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithOneNullProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommand
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("ac9f38a968a06d319715d431fe9ac1440a2493ea6e2e49904edb82dc117b102f", ShaType.Sha256)]
        [TestCase("695bc5c39310f6dfbebdab42ecb1906b8ecda2ec9e8221d26f7b5f17584c22a7b99ea42e8dcb11c510a4d15c0b60e0bef362b7e1c55c1deaee87cb4aa6477e0e", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollection_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase("2be30945cea02d0d3bd496a889513e0e67b4fbbcc00c0e8cf453be8ef79e430a", ShaType.Sha256)]
        [TestCase("93e81666b6b4a0ae2d409c760d8c7e48a54fc1abb8cc2fec4561454416107a8333dbb48eb3874557412def490c6154cb9da4fc4edf592289791e16866eb3a3a1", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollectionWithOneEmptyProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("2be30945cea02d0d3bd496a889513e0e67b4fbbcc00c0e8cf453be8ef79e430a", ShaType.Sha256)]
        [TestCase("93e81666b6b4a0ae2d409c760d8c7e48a54fc1abb8cc2fec4561454416107a8333dbb48eb3874557412def490c6154cb9da4fc4edf592289791e16866eb3a3a1", ShaType.Sha512)]
        public void GetSignature_InputHasTwoLevelsOfPropertiesWithCollectionWithOneNullProperty_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new TwoLevelsPropertyRequestCommandWithCollection
            {
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
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("Only two levels of reference types are allowed for signature calculation.", ShaType.Sha256)]
        [TestCase("Only two levels of reference types are allowed for signature calculation.", ShaType.Sha512)]
        public void GetSignature_InputHasThreeLevelsOfPropertiesWithCollection_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new ThreeLevelsPropertyRequestCommand
            {
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
            var actualException = Assert.Throws<SignatureException>(() => service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType));
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
                Language = "TestLanguage",
                Signature = "TestSignature",
                ZBank = "Banca Transilvania",
                XAuthenticationCode = "1234"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("91378d427e279fbf4dc81e87c59655b9f25553f09ef9dd2d444853bb2b46b881", ShaType.Sha256)]
        [TestCase("69906e0c214a10b929df06ce146f5d0e126c6cb365231b459cc00385251839044bd10bd1575dc4b19022fcc363c44d6c3d5efb3137295dc692724f126898c47d", ShaType.Sha512)]
        public void GetSignature_AuthorizeRequestCommand_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("a450f20c83c1c7b4576f253041907223ff7e376585175d70896dfa547ca6c401", ShaType.Sha256)]
        [TestCase("09f67e4b8936f53c7c33ba91dbdc4e5cb3c2a0dc31ad4fb016e09dd5e0d14155553982f05fb317566901d8b6c36544c3061f62b87517d0040d006c3feaef57d5", ShaType.Sha512)]
        public void GetSignature_PurchaseRequestCommand_ReturnsCorrectSignature(string expectedResult, ShaType shaType)
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature"
            };
            var service = new SignatureProvider();

            //act
            var actualResult = service.GetSignature(objectTest, SdkConfiguration.RequestShaPhrase, shaType);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
