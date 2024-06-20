using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Web
{
    public class HtmlProviderTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new Mock<ILoggerFactory>();
        private readonly Mock<ILogger<HtmlProvider>> _loggerMock = new();
        [SetUp]
        public void Setup()
        {
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            SdkConfiguration.Configure(
                FilePathMerchantConfiguration,
                _loggerFactoryMock.Object);
        }

        #region GetHtmlForRedirectIntegration_Authorization
        [Test]
        public void GetHtmlForRedirectIntegration_Authorization_ReturnFormPostWithMandatoryFields()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='command' value=\"AUTHORIZATION\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"TestCurrency\">" +
                "<input type='hidden' name='customer_email' value=\"TestEmail\">" +
                "<input type='hidden' name='app_programming' value=\".NET\">" +
                "<input type='hidden' name='app_plugin' value=\".dotNETSDK\">" +
                "<input type='hidden' name='app_plugin_version' value=\"v2.1.0\">" +
                "<input type='hidden' name='app_ver' value=\"1.0.0.0\">" +
                "<input type='hidden' name='app_framework' value=\".NET\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"1317e9815b68033efeefb018c5d76e522657ea0d28dd0c3cd868314172860c72\"></form>";
            var actualResult = service.GetHtmlForRedirectIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [Test]
        public void GetHtmlForRedirectIntegration_Authorization_ReturnFormPostWithOneOfMandatoryFieldsMissing()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail"
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => service.GetHtmlForRedirectIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void GetHtmlForRedirectIntegration_Authorization_ReturnFormPostWithOptinalFields()
        {
            //arrange
            var objectTest = new AuthorizeRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail",
                RememberMe = "yes",
                Description = "TestDescription",
                CustomerName = "TestCustomer"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='command' value=\"AUTHORIZATION\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"TestCurrency\">" +
                "<input type='hidden' name='customer_email' value=\"TestEmail\">" +
                "<input type='hidden' name='order_description' value=\"TestDescription\">" +
                "<input type='hidden' name='customer_name' value=\"TestCustomer\">" +
                "<input type='hidden' name='remember_me' value=\"YES\">" +
                "<input type='hidden' name='app_programming' value=\".NET\">" +
                "<input type='hidden' name='app_plugin' value=\".dotNETSDK\">" +
                "<input type='hidden' name='app_plugin_version' value=\"v2.1.0\">" +
                "<input type='hidden' name='app_ver' value=\"1.0.0.0\">" +
                "<input type='hidden' name='app_framework' value=\".NET\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"6c95de0aa2627d8ebc72a8b24e3bd94e849554dc3344c605e7e57f5d2dbe57a8\"></form>";
            var actualResult = service.GetHtmlForRedirectIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion

        #region GetHtmlForRedirectIntegration_Purchase
        [Test]
        public void GetHtmlForRedirectIntegration_Purchase_ReturnFormPostWithMandatoryFields()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='command' value=\"PURCHASE\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"TestCurrency\">" +
                "<input type='hidden' name='customer_email' value=\"TestEmail\">" +
                "<input type='hidden' name='app_programming' value=\".NET\">" +
                "<input type='hidden' name='app_plugin' value=\".dotNETSDK\">" +
                "<input type='hidden' name='app_plugin_version' value=\"v2.1.0\">" +
                "<input type='hidden' name='app_ver' value=\"1.0.0.0\">" +
                "<input type='hidden' name='app_framework' value=\".NET\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"fd4146b2a8ea579bba48c2cc40b2e2ee8f63dc0e5dea0cf39ead84b165ebd2e1\"></form>";
            var actualResult = service.GetHtmlForRedirectIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [Test]
        public void GetHtmlForRedirectIntegration_Purchase_ReturnFormPostWithOneOfMandatoryFieldsMissing()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail"
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => service.GetHtmlForRedirectIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void GetHtmlForRedirectIntegration_Purchase_ReturnFormPostWithOptinalFields()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Currency = "TestCurrency",
                Amount = 24.3,
                CustomerEmail = "TestEmail",
                RememberMe = "yes",
                Description = "TestDescription",
                CustomerName = "TestCustomer"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='command' value=\"PURCHASE\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"TestCurrency\">" +
                "<input type='hidden' name='customer_email' value=\"TestEmail\">" +
                "<input type='hidden' name='order_description' value=\"TestDescription\">" +
                "<input type='hidden' name='customer_name' value=\"TestCustomer\">" +
                "<input type='hidden' name='remember_me' value=\"YES\">" +
                "<input type='hidden' name='app_programming' value=\".NET\">" +
                "<input type='hidden' name='app_plugin' value=\".dotNETSDK\">" +
                "<input type='hidden' name='app_plugin_version' value=\"v2.1.0\">" +
                "<input type='hidden' name='app_ver' value=\"1.0.0.0\">" +
                "<input type='hidden' name='app_framework' value=\".NET\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"88c67155d64616a7e2071cbad9442c63be7c93f2680cd5392fd2d80d8f3a46bf\"></form>";
            var actualResult = service.GetHtmlForRedirectIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion

        #region GetHtmlTokenizationForStandardIframeIntegration
        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_AllMandatoryFieldsAreProvided_ReturnsIframe()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<iframe name='apsFrame' src='' width='600' height='440'></iframe><br>" +
                "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' id='apsIframeForm' target='apsFrame'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"270a246f69b0f83a99a36a77ec8fb20bec2f5bd90c9a2006480c68224e15146e\">" +
                "<input value='Place Order' type='submit' id='apsIframeForm'></form>";
            var actualResult = service.GetHtmlTokenizationForStandardIframeIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_OneOfMandatoryFieldsMissing_ThrowsException()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => service.GetHtmlTokenizationForStandardIframeIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_OneOptionalFieldIsProvided_ReturnsIframe()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                ReturnUrl = "TestReturnUrl"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<iframe name='apsFrame' src='' width='600' height='440'></iframe><br>" +
                "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' id='apsIframeForm' target='apsFrame'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='return_url' value=\"TestReturnUrl\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"2b3ca635e65d634f2d72623b448799648f4c914785c149089fc342aef1c3c5cd\">" +
                "<input value='Place Order' type='submit' id='apsIframeForm'></form>";
            var actualResult = service.GetHtmlTokenizationForStandardIframeIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion

        #region GetHtmlTokenizationForCustomIntegration
        [Test]
        public void GetHtmlTokenizationForCustomIntegration_AllMandatoryFieldsAreProvided_ReturnsFormPost()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.PayFort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"270a246f69b0f83a99a36a77ec8fb20bec2f5bd90c9a2006480c68224e15146e\"><br>" +
                "<label>Card Number</label><br>" +
                "<input name='card_number' value=\"\"><br>" +
                "<label>Expiry Date</label><br>" +
                "<input name='expiry_date' value=\"\"><br>" +
                "<label>Security Code</label><br>" +
                "<input name='card_security_code' value=\"\"><br>" +
                "<label>Cardholder Name</label><br>" +
                "<input name='card_holder_name' value=\"\"><br>" +
                "<input value='Place Order' type='submit' id='redirectToApsForm'></form>";
            var actualResult = service.GetHtmlTokenizationForCustomIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [Test]
        public void GetHtmlTokenizationForCustomIntegration_OneOfMandatoryFieldsMissing_ThrowsException()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                Language = "TestLanguage",
                Signature = "TestSignature",
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "MerchantReference is mandatory (Parameter 'MerchantReference')";
            var actualException = Assert.Throws<ArgumentNullException>(() => service.GetHtmlTokenizationForCustomIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        [Test]
        public void GetHtmlTokenizationForCustomIntegration_OneOptionalFieldIsProvided_ReturnsFormPost()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                ReturnUrl = "TestReturnUrl"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.PayFort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='return_url' value=\"TestReturnUrl\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"2b3ca635e65d634f2d72623b448799648f4c914785c149089fc342aef1c3c5cd\"><br>" +
                "<label>Card Number</label><br>" +
                "<input name='card_number' value=\"\"><br>" +
                "<label>Expiry Date</label><br>" +
                "<input name='expiry_date' value=\"\"><br>" +
                "<label>Security Code</label><br>" +
                "<input name='card_security_code' value=\"\"><br>" +
                "<label>Cardholder Name</label><br>" +
                "<input name='card_holder_name' value=\"\"><br>" +
                "<input value='Place Order' type='submit' id='redirectToApsForm'></form>";
            var actualResult = service.GetHtmlTokenizationForCustomIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion
    }
}
