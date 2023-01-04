using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Signature;
using APS.DotNetSDK.Web.ApplePayIntegration;
using Microsoft.Extensions.DependencyInjection;
using Environment = APS.DotNetSDK.Configuration.Environment;

namespace APS.DotNetSDK.Tests.Web.ApplePayIntegration
{
    public class JavascriptProviderTests
    {
        private const string FilePath = @"Web\ApplePayIntegration\Certificate.pem";
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";

        private readonly LoggingConfiguration _loggingConfiguration = new LoggingConfiguration(new ServiceCollection(),
            @"Logging/Config/SerilogConfig.json", "APS.DotNetSDK");
        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(FilePathMerchantConfiguration,
                _loggingConfiguration,
                new ApplePayConfiguration(
                    new X509Certificate2(FilePath)));
        }

        [Test]
        public async Task GetJavaScriptForApplePayIntegrationWithoutSupportedCountries_ReturnsSession_InputIsValid()
        {
            //arrange
            var ajaxSessionValidationUrl = "http://test1.com";
            var ajaxCommandUrl = "http://test.com";
            var countryCode = "AE";
            var currencyCode = "AED";
            var supportedNetworks = new List<string>
            {
                "visa", "masterCard", "amex", "mada"
            };

            JavascriptProvider provider = new JavascriptProvider();

            //act
            var actualJavascriptContent = provider.GetJavaScriptForApplePayIntegration(ajaxSessionValidationUrl, ajaxCommandUrl, countryCode,
                currencyCode, supportedNetworks);

            //assert
            var expectedJavascriptContent = await File.ReadAllTextAsync(@"Web\ApplePayIntegration\ExpectedJavascriptWithoutSupportedCountries.txt");

            Assert.That(actualJavascriptContent, Is.EqualTo(expectedJavascriptContent));
        }

        [Test]
        public async Task GetJavaScriptForApplePayIntegrationWithSupportedContries_ReturnsSession_InputIsValid()
        {
            //arrange
            var ajaxSessionValidationUrl = "http://test1.com";
            var ajaxCommandUrl = "http://test.com";
            var countryCode = "AE";
            var currencyCode = "AED";
            var supportedNetworks = new List<string>
            {
                "visa", "masterCard", "amex", "mada"
            };
            var supportedCountries = new List<string>
            {
                "AE", "RO"
            };
            JavascriptProvider provider = new JavascriptProvider();

            //act
            var actualJavascriptContent = provider.GetJavaScriptForApplePayIntegration(ajaxSessionValidationUrl, ajaxCommandUrl, countryCode,
                currencyCode, supportedNetworks, supportedCountries);

            //assert
            var expectedJavascriptContent = await File.ReadAllTextAsync(@"Web\ApplePayIntegration\ExpectedJavascriptWithSupportedCountries.txt");

            Assert.That(actualJavascriptContent, Is.EqualTo(expectedJavascriptContent));
        }


        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_AjaxSesionValidationUrlIsNull()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentNullException>(() => provider.GetJavaScriptForApplePayIntegration(
                null,
                "http://test.com",
                "AE",
                "AED",
                new List<string>
                {
                    "visa", "masterCard", "amex", "mada"
                }));
        }

        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_AjaxCommandUrlIsNull()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentNullException>(() => provider.GetJavaScriptForApplePayIntegration(
                "http://test.com",
                null,
                "AE",
                "AED",
                new List<string>
                {
                    "visa", "masterCard", "amex", "mada"
                }));
        }

        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_CountryCodeIsNull()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentNullException>(() => provider.GetJavaScriptForApplePayIntegration(
                "http://test.com",
                "http://test.com",
                null,
                "AED",
                new List<string>
                {
                    "visa", "masterCard", "amex", "mada"
                }));
        }

        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_CurrencyCodeIsEmpty()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentNullException>(() => provider.GetJavaScriptForApplePayIntegration(
                "http://test.com",
                "http://test.com",
                "AE",
                "",
                new List<string>
                {
                    "visa", "masterCard", "amex", "mada"
                }));
        }

        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_SupportedNetworksIsNull()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentNullException>(() => provider.GetJavaScriptForApplePayIntegration(
                "http://test.com",
                "http://test.com",
                "AE",
                "AED",
                null));
        }

        [Test]
        public void GetJavaScriptForApplePayIntegration_ThrowsException_SupportedNetworksIsEmpty()
        {
            //arrange
            JavascriptProvider provider = new JavascriptProvider();

            //act
            //assert
            Assert.Throws<ArgumentException>(() => provider.GetJavaScriptForApplePayIntegration(
                "http://test.com",
                "http://test.com",
                "AE",
                "AED",
                new List<string>()));
        }
    }
}