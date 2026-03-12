using System;
using System.Text;
using System.Linq;
using System.Reflection;
using APS.DotNetSDK.Utils;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using APS.DotNetSDK.Commands.Requests;
using APS.Signature;
using APS.Signature.Utils;

namespace APS.DotNetSDK.Web
{
    public class HtmlProvider : IHtmlProvider
    {
        private readonly ILogger<HtmlProvider> _logger;
        private ApsConfiguration _apsConfiguration;
        private readonly SignatureProvider _signatureProvider;

        /// <summary>
        /// Constructor for Html Provider
        /// </summary>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when sdk configuration is not set</exception>
        public HtmlProvider()
        {
            SdkConfiguration.Validate();

            _logger = SdkConfiguration.LoggerFactory.CreateLogger<HtmlProvider>();
            _signatureProvider = new SignatureProvider();
        }

        public string GetHtmlForRedirectIntegration(AuthorizeRequestCommand command, string accountName = null)
        {
            var account = GetCommandAccountDetails(command, accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            var formActionUrl = _apsConfiguration.GetEnvironmentConfiguration().RedirectUrl;
            var redirectFormPostTemplate = _apsConfiguration.GetRedirectFormPostTemplate();

            return BuildHtmlFormPost(command, formActionUrl, redirectFormPostTemplate, account);
        }

        public string GetHtmlForRedirectIntegration(PurchaseRequestCommand command, string accountName = null)
        {
            var account = GetCommandAccountDetails(command, accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            var formActionUrl = _apsConfiguration.GetEnvironmentConfiguration().RedirectUrl;
            var redirectFormPostTemplate = _apsConfiguration.GetRedirectFormPostTemplate();

            return BuildHtmlFormPost(command, formActionUrl, redirectFormPostTemplate, account);
        }

        public string GetHtmlTokenizationForStandardIframeIntegration(TokenizationRequestCommand command, string accountName = null)
        {
            var account = GetCommandAccountDetails(command, accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            var formActionUrl = _apsConfiguration.GetEnvironmentConfiguration().StandardCheckoutActionUrl;
            var iframeFormPostTemplate = _apsConfiguration.GetStandardIframeFormPostTemplate();

            return BuildHtmlFormPost(command, formActionUrl, iframeFormPostTemplate, account);
        }

        public string GetHtmlTokenizationForCustomIntegration(TokenizationRequestCommand command, string accountName = null)
        {
            var account = GetCommandAccountDetails(command, accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            var formActionUrl = _apsConfiguration.GetEnvironmentConfiguration().CustomCheckoutActionUrl;
            var customFormPostTemplate = _apsConfiguration.GetCustomFormPostTemplate();

            ValidateMandatoryProperties(command);

            command.Signature = CreateSignature(command, account);

            var properties = typeof(TokenizationRequestCommand).GetProperties();

            var hiddenFieldsFormPost = BuildHiddenFieldsForForm(command, properties);
            var cardDetailsFields = BuildCardFieldsForForm(properties);

            var formPostForReturn = string.Format(customFormPostTemplate, formActionUrl, hiddenFieldsFormPost, cardDetailsFields);

            return formPostForReturn;
        }

        public string GetJavaScriptToCloseModal(string accountName = null)
        {
            var account = SdkConfiguration.GetAccount(accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            return _apsConfiguration.GetCloseModalJavaScript();
        }

        public string Handle3dsSecure(string secure3dsUrl = null, bool useModal = false, bool standardCheckout = false, string accountName = null)
        {
            var account = SdkConfiguration.GetAccount(accountName);
            _apsConfiguration = new ApsConfiguration(account.IsTestEnvironment);
            var closeIframe = _apsConfiguration.GetCloseIframeJavaScript();

            if (!string.IsNullOrEmpty(secure3dsUrl))
            {
                var sanitizedUrl = ValidateAndSanitizeUrl(secure3dsUrl);

                var stringBuilder = new StringBuilder();
                if (useModal)
                {
                    return BuildModalFor3ds(sanitizedUrl, standardCheckout, closeIframe);
                }

                var jsEncodedUrl = JavaScriptStringEncode(sanitizedUrl);
                stringBuilder.Append($"<script>window.parent.location.href = '{jsEncodedUrl}';</script>");
                stringBuilder.Append(closeIframe);

                return stringBuilder.ToString();
            }

            return closeIframe;
        }
        

        private string BuildModalFor3ds(string secure3dsUrl, bool standardCheckout, string closeIframe)
        {
            var stringBuilder = new StringBuilder();
            if (standardCheckout)
            {
                stringBuilder = BuildModalForStandardCheckout(secure3dsUrl, closeIframe);
                return stringBuilder.ToString();
            }

            stringBuilder.Append(GetModalContent(secure3dsUrl));
            stringBuilder.Append($"<script>{GetModalJavaScript()}</script>");

            return stringBuilder.ToString();
        }

        public string GetJavaScriptForFingerPrint()
        {
            var fingerPrintContent = FileReader
                .GetEmbeddedResourceContent("APS.DotNetSDK.Web.FingerPrintJavaScript.txt");

            return fingerPrintContent;
        }

        #region private methods
        private SdkConfigurationDto GetCommandAccountDetails<TRequest>(TRequest command, string nameAccount = null)
            where TRequest : RequestCommand
        {
            var account = SdkConfiguration.GetAccount(nameAccount);
            command.AccessCode = account.AccessCode;
            command.MerchantIdentifier = account.MerchantIdentifier;
            return account;
        }

        private string BuildHtmlFormPost<T>(T command, string formActionUrl, string formPostTemplate, SdkConfigurationDto account) where T : RequestCommand
        {
            ValidateMandatoryProperties(command);

            command.Signature = CreateSignature(command, account);

            var properties = typeof(T).GetProperties();

            var hiddenFieldsFormPost = BuildHiddenFieldsForForm(command, properties);
            var formPostForReturn = string.Format(formPostTemplate, formActionUrl, hiddenFieldsFormPost);

            return formPostForReturn;
        }

        private string CreateSignature<T>(T command, SdkConfigurationDto account) where T : RequestCommand
        {
            _logger.LogInformation($"Starting signature calculation for [MerchantReference:{command.MerchantReference}]");
            _logger.LogDebug($"Starting signature calculation for [MerchantReference:{command.MerchantReference},RequestObject:{@command.ToAnonymizedJson()}]");

            var signature = _signatureProvider.GetSignature(command, account.RequestShaPhrase, account.ShaType);

            _logger.LogInformation($"Generated signature for [MerchantReference:{command.MerchantReference},Signature:{signature}]");
            return signature;
        }

        private static string BuildHiddenFieldsForForm<T>(T command, IEnumerable<PropertyInfo> properties) where T : RequestCommand
        {
            var hiddenFieldsFormPost = new StringBuilder();

            foreach (var prop in properties)
            {
                var jsonPropertyName = prop.GetJsonPropertyName();
                var propertyValue = prop.GetValue(command, null);

                if (jsonPropertyName == "installments" && propertyValue != null)
                {
                    if (!(propertyValue.ToString() == "STANDALONE"))
                    {
                        throw new ArgumentException("Installments should be \"STANDALONE\"", $"Installments");
                    }
                }

                if (propertyValue == null || propertyValue.ToString() == string.Empty || propertyValue.ToString() == "0")
                {
                    continue;
                }

                // FIX: HTML-encode both name and value to prevent XSS
                var encodedName = WebUtility.HtmlEncode(jsonPropertyName);
                var encodedValue = WebUtility.HtmlEncode(propertyValue.ToString());
                hiddenFieldsFormPost.Append($"<input type='hidden' name='{encodedName}' value=\"{encodedValue}\">");
            }

            return hiddenFieldsFormPost.ToString().Trim();
        }

        private static string BuildCardFieldsForForm(IEnumerable<PropertyInfo> properties)
        {
            var hiddenFieldsFormPost = new StringBuilder();

            hiddenFieldsFormPost.Append(BuildCardFieldForForm(properties, "Card Number", "card_number"));
            hiddenFieldsFormPost.Append(BuildCardFieldForForm(properties, "Expiry Date", "expiry_date"));
            hiddenFieldsFormPost.Append(BuildCardFieldForForm(properties, "Security Code", "card_security_code"));
            hiddenFieldsFormPost.Append(BuildCardFieldForForm(properties, "Cardholder Name", "card_holder_name"));

            return hiddenFieldsFormPost.ToString().Trim();
        }

        private static string BuildCardFieldForForm(IEnumerable<PropertyInfo> properties, string labelName, string propertyName)
        {
            var property = properties.First(x => x.GetJsonPropertyName() == propertyName);

            var jsonPropertyName = property.GetJsonPropertyName();
            return $"<label>{labelName}</label><br><input name='{jsonPropertyName}' value=\"\"><br>";
        }

        private void ValidateMandatoryProperties<T>(T command) where T : RequestCommand
        {
            _logger.LogInformation($"Validation of mandatory properties for [MerchantReference:{command.MerchantReference}]");

            command.ValidateMandatoryProperties();

            _logger.LogInformation($"Validation successfully for [MerchantReference:{command.MerchantReference}]");
        }

        private string GetModalContent(string secure3dsUrl)
        {
            _logger.LogDebug("Started to read modal template from file.");
            var modalFileText = FileReader
                .GetEmbeddedResourceContent("APS.DotNetSDK.Web.Modal3DSTemplate.txt");
            _logger.LogDebug($"Read template from file {modalFileText}");

            // URL scheme validation is now handled by ValidateAndSanitizeUrl
            // called in Handle3dsSecure before this method is reached.
            // Keep URI structure validation as defense-in-depth.
            var result = Uri.TryCreate(secure3dsUrl, UriKind.Absolute, out _);
            if (result == false)
            {
                throw new ArgumentException("Please provide a valid url");
            }

            _logger.LogDebug("Started to build the string for modal content");
            var builder = new StringBuilder(modalFileText);

            // HTML-encode the URL for safe insertion into the iframe src attribute
            var htmlEncodedUrl = WebUtility.HtmlEncode(secure3dsUrl);
            builder.Replace("[IframeSource]", htmlEncodedUrl);

            return builder.ToString();
        }

        private static string GetModalJavaScript()
        {
            var modalJavaScriptContent = FileReader
                .GetEmbeddedResourceContent("APS.DotNetSDK.Web.Modal3DSJavaScriptTemplate.txt");

            return modalJavaScriptContent;
        }

        private StringBuilder BuildModalForStandardCheckout(string secure3dsUrl, string closeIframe)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<script>");
            stringBuilder.Append("var elemDiv = document.createElement('div');");

            var modalContent = GetModalContent(secure3dsUrl);
            // JS-encode the modal HTML content for safe insertion into a JS string literal
            var jsEncodedModalContent = JavaScriptStringEncode(modalContent.Trim());
            stringBuilder.Append($"elemDiv.innerHTML='{jsEncodedModalContent}';");

            stringBuilder.Append("var script = document.createElement('script');");

            var modalJavaScriptContent = GetModalJavaScript();
            // JS-encode the JavaScript content for safe insertion into a JS string literal
            var jsEncodedScriptContent = JavaScriptStringEncode(modalJavaScriptContent.Trim());
            stringBuilder.Append($"script.innerHTML='{jsEncodedScriptContent}';");

            stringBuilder.Append("elemDiv.appendChild(script);");
            stringBuilder.Append("window.parent.document.body.appendChild(elemDiv);</script>");
            stringBuilder.Append(closeIframe);

            return stringBuilder;
        }

        /// <summary>
        /// Validates that the URL is a well-formed absolute URI with an allowed scheme (http or https only).
        /// Rejects javascript:, data:, vbscript:, and all other non-HTTP(S) schemes to prevent XSS.
        /// </summary>
        /// <param name="url">The URL to validate</param>
        /// <returns>The validated URL string</returns>
        /// <exception cref="ArgumentException">Thrown when the URL is not valid or uses a disallowed scheme</exception>
        private static string ValidateAndSanitizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Please provide a valid url");
            }

            // Only allow http and https schemes - reject javascript:, data:, vbscript:, etc.
            var scheme = uri.Scheme.ToLowerInvariant();
            if (scheme != "http" && scheme != "https")
            {
                throw new ArgumentException(
                    $"Only http and https URL schemes are allowed. The provided URL uses the '{uri.Scheme}' scheme.");
            }

            return url;
        }

        /// <summary>
        /// Encodes a string for safe insertion into a JavaScript single-quoted string literal.
        /// Escapes characters that could break out of the string context or introduce script injection.
        /// </summary>
        /// <param name="value">The string to encode</param>
        /// <returns>The JavaScript-encoded string (without surrounding quotes)</returns>
        private static string JavaScriptStringEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var sb = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\'':
                        sb.Append("\\'");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '<':
                        // Prevent </script> breakout within inline script blocks
                        sb.Append("\\u003c");
                        break;
                    case '>':
                        sb.Append("\\u003e");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}
