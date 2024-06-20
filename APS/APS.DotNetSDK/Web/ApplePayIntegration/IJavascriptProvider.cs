using System.Collections.Generic;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public interface IJavascriptProvider
    {
        /// <summary>
        /// Retrieve javascript for ApplePay integration
        /// </summary>
        /// <param name="ajaxSessionValidationUrl">The ajax session validation url </param>
        /// <param name="ajaxCommandUrl">The ajax command url</param>
        /// <param name="countryCode">The country code</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="supportedNetworks">The supported networks</param>
        /// <param name="supportedCountries">The supported countries</param>
        /// <returns>The javascript to inject into the DOM</returns>
        /// <exception cref="System.ArgumentNullException">Get the exception when the mandatory parameters are not sent</exception>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when the mandatory properties are not sent in SDK configuration</exception>
        string GetJavaScriptForApplePayIntegration(string ajaxSessionValidationUrl, string ajaxCommandUrl, string countryCode, string currencyCode,
            IEnumerable<string> supportedNetworks, IEnumerable<string> supportedCountries = null, string nameAccount = null);
    }
}
