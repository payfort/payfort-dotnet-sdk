using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using APS.DotNetSDK.Configuration;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public class JavascriptProvider : IJavascriptProvider
    {
        private static readonly string JavascriptTemplateFile = $@"Configuration{Path.DirectorySeparatorChar}ApplePayJavascriptTemplate.txt";

        public string GetJavaScriptForApplePayIntegration(string ajaxSessionValidationUrl, string ajaxCommandUrl, string countryCode,
            string currencyCode, IEnumerable<string> supportedNetworks, IEnumerable<string> supportedCountries = null, string nameAccount = null)
        {
            var account = SdkConfiguration.GetAccount(nameAccount);

            SdkConfiguration.Validate();
            SdkConfiguration.ValidateApplePayConfiguration(account);

            if (string.IsNullOrEmpty(ajaxSessionValidationUrl))
            {
                throw new ArgumentNullException($"ajaxSessionValidationUrl",
                    "ajaxSessionValidationUrl is needed for ApplePay Javascript provider");
            }

            if (string.IsNullOrEmpty(ajaxCommandUrl))
            {
                throw new ArgumentNullException($"ajaxCommandUrl",
                    "ajaxCommandUrl is needed for ApplePay Javascript provider");
            }

            if (string.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException($"countryCode",
                    "countryCode is needed for ApplePay Javascript provider");
            }

            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException($"currencyCode",
                    "currencyCode is needed for ApplePay Javascript provider");
            }

            if (supportedNetworks == null)
            {
                throw new ArgumentNullException($"supportedNetworks",
                    "supportedNetworks are required for ApplePay Javascript provider");
            }

            if (!supportedNetworks.Any())
            {
                throw new ArgumentException("supportedNetworks are required for ApplePay Javascript provider",
                    $"supportedNetworks");
            }

            string javascriptFileContent = File.ReadAllText(JavascriptTemplateFile);

            StringBuilder builder = new StringBuilder(javascriptFileContent);

            builder.Replace("[AjaxSessionValidationUrl]", ajaxSessionValidationUrl);
            builder.Replace("[AjaxCommandUrl]", ajaxCommandUrl);
            builder.Replace("[CountryCode]", countryCode);
            builder.Replace("[CurrencyCode]", currencyCode);

            string supportedNetworksArrayAsString = GetAsJavascriptArray(supportedNetworks);
            builder.Replace("[SupportedNetworks]", supportedNetworksArrayAsString);

            if (supportedCountries != null && supportedCountries.Any())
            {
                string supportedCountriesArrayAsString = GetAsJavascriptArray(supportedCountries);
                builder.Replace("[SupportedCountries]", $"supported_countries: {supportedCountriesArrayAsString},");
            }
            else
            {
                builder.Replace("[SupportedCountries]", string.Empty);
            }

            builder.Replace("[DisplayName]", account.ApplePayConfiguration.DisplayName);

            return builder.ToString();
        }

        #region private methods
        private string GetAsJavascriptArray(IEnumerable<string> array)
        {
            if (array != null && array.Any())
            {
                return $"[{string.Join(", ", array.Select(x => $"'{x}'"))}]";
            }

            return "[]";
        }
        #endregion
    }
}