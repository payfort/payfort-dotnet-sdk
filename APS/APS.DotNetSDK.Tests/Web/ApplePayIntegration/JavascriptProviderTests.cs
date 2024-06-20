using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web.ApplePayIntegration;
using Microsoft.Extensions.Logging;
using Moq;

namespace APS.DotNetSDK.Tests.Web.ApplePayIntegration
{
    public class JavascriptProviderTests
    {
        private readonly string _filePath = $"Web{ Path.DirectorySeparatorChar.ToString() }ApplePayIntegration{ Path.DirectorySeparatorChar.ToString() }Certificate.pem";
        private const string FilePathMerchantConfiguration = @"Configuration\MerchantSdkConfiguration.json";

        private Mock<ILoggerFactory> _loggerFactory = new Mock<ILoggerFactory>();
        [SetUp]
        public void Setup()
        {
            SdkConfiguration.Configure(FilePathMerchantConfiguration,
                _loggerFactory.Object,
                new ApplePayConfiguration(
                    new X509Certificate2(_filePath)));
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
            var expectedJavascriptContent = await File.ReadAllTextAsync($"Web{ Path.DirectorySeparatorChar.ToString() }ApplePayIntegration{ Path.DirectorySeparatorChar.ToString() }ExpectedJavascriptWithoutSupportedCountries.txt");

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
            var expectedJavascriptContent = await File.ReadAllTextAsync($"Web{ Path.DirectorySeparatorChar.ToString() }ApplePayIntegration{ Path.DirectorySeparatorChar.ToString() }ExpectedJavascriptWithSupportedCountries.txt");

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