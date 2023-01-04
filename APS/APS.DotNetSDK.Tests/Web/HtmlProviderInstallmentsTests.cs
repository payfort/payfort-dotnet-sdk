using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web;
using Microsoft.Extensions.DependencyInjection;

namespace APS.DotNetSDK.Tests.Web
{
    public class HtmlProviderInstallmentsTests
    {
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";
        [SetUp]
        public void Setup()
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration(new ServiceCollection(), @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");

            SdkConfiguration.Configure(FilePathMerchantConfiguration,
                loggingConfiguration);
        }
        #region GetHtmlForRedirectIntegration_Installments_Purchase
        [Test]
        public void GetHtmlForRedirectIntegration_Installments_Purchase_ReturnFormPostWithMandatoryFields()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail",
                Installments = "STANDALONE"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' name='redirectToApsForm'><br>" +
                "<input type='hidden' name='command' value=\"PURCHASE\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"TestCurrency\">" +
                "<input type='hidden' name='customer_email' value=\"TestEmail\">" +
                "<input type='hidden' name='installments' value=\"STANDALONE\">" +
                "<input type='hidden' name='app_programming' value=\".NET\">" +
                "<input type='hidden' name='app_plugin' value=\".dotNETSDK\">" +
                "<input type='hidden' name='app_plugin_version' value=\"v2.0.0\">" +
                "<input type='hidden' name='app_ver' value=\"1.0.0.0\">" +
                "<input type='hidden' name='app_framework' value=\".NET\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"1c92232101516677cadbeb5caed537b2aeea17f4c4cd5fece4c7c60b485c9c10\"></form>";
            var actualResult = service.GetHtmlForRedirectIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetHtmlForRedirectIntegration_Installments_Purchase_ReturnErrorWhenInstallmentsIsDifferent()
        {
            //arrange
            var objectTest = new PurchaseRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Amount = 24.3,
                Currency = "TestCurrency",
                CustomerEmail = "TestEmail",
                Installments = "TestInstallments"
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "Installments should be \"STANDALONE\",\"YES\", \"NO\" or \"HOSTED\" (Parameter 'Installments')";
            var actualException = Assert.Throws<ArgumentException>(() => service.GetHtmlForRedirectIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }
        #endregion

        #region GetHtmlTokenizationForStandardIframeIntegration
        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_Installments_AllMandatoryFieldsAreProvided_ReturnsIframe()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Installments = "Standalone",
                Amount = 24.3,
                Currency = "AED",
                CardHolderName = "Andreea's"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<iframe name='apsFrame' src='' width='600' height='440'></iframe><br>" +
                "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' id='apsIframeForm' target='apsFrame'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='card_holder_name' value=\"Andreea's\">" +
                "<input type='hidden' name='installments' value=\"STANDALONE\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"AED\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"c46fb7fb6b1973948fd75191b37b49c24f2cc7c165e750b103f365411c01a3cd\">" +
                "<input value='Place Order' type='submit' id='apsIframeForm'></form>";
            var actualResult = service.GetHtmlTokenizationForStandardIframeIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_Installments_OneOfMandatoryFieldsMissing_ThrowsException()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Installments = "Standalone",
                Currency = "AED"
            };
            var service = new HtmlProvider();

            //act
            //assert
            var expectedException = "Amount is mandatory for installments (Parameter 'Amount')";
            var actualException = Assert.Throws<ArgumentNullException>(() => service.GetHtmlTokenizationForStandardIframeIntegration(objectTest));
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void GetHtmlTokenizationForStandardIframeIntegration_Installments_OneOptionalFieldIsProvided_ReturnsIframe()
        {
            //arrange
            var objectTest = new TokenizationRequestCommand
            {
                MerchantReference = "TestMerchantReference",
                Language = "TestLanguage",
                Signature = "TestSignature",
                Installments = "Standalone",
                Amount = 24.3,
                Currency = "AED",
                CustomerCountryCode = "TestCustomerCountryCode"
            };
            var service = new HtmlProvider();

            //act
            var expectedResult = "<iframe name='apsFrame' src='' width='600' height='440'></iframe><br>" +
                "<form action='https://sbcheckout.payfort.com/FortAPI/paymentPage' method='post' id='apsIframeForm' target='apsFrame'><br>" +
                "<input type='hidden' name='service_command' value=\"TOKENIZATION\">" +
                "<input type='hidden' name='installments' value=\"STANDALONE\">" +
                "<input type='hidden' name='amount' value=\"24.3\">" +
                "<input type='hidden' name='currency' value=\"AED\">" +
                "<input type='hidden' name='customer_country_code' value=\"TestCustomerCountryCode\">" +
                "<input type='hidden' name='access_code' value=\"TestAccessCode\">" +
                "<input type='hidden' name='merchant_identifier' value=\"TestMerchantIdentifier\">" +
                "<input type='hidden' name='merchant_reference' value=\"TestMerchantReference\">" +
                "<input type='hidden' name='language' value=\"testlanguage\">" +
                "<input type='hidden' name='signature' value=\"11b7e06d1d391a12719cc2820eca528480a10a5c30ca98ca372ecfa75fdb1ecd\">" +
                "<input value='Place Order' type='submit' id='apsIframeForm'></form>";
            var actualResult = service.GetHtmlTokenizationForStandardIframeIntegration(objectTest);

            //assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion
    }
}
