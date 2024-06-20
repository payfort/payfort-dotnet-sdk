using System;
using System.Text;
using System.Linq;
using System.Reflection;
using APS.DotNetSDK.Utils;
using System.Collections.Generic;
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
                var stringBuilder = new StringBuilder();
                if (useModal)
                {
                    return BuildModalFor3ds(secure3dsUrl, standardCheckout, closeIframe);
                }

                stringBuilder.Append($"<script>window.parent.location.href = '{secure3dsUrl}';</script>");
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

                hiddenFieldsFormPost.Append($"<input type='hidden' name='{jsonPropertyName}' value=\"{propertyValue}\">");
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

            var result = Uri.TryCreate(secure3dsUrl, UriKind.Absolute, out _);
            if (result == false)
            {
                throw new ArgumentException("Please provide a valid url");
            }

            _logger.LogDebug("Started to build the string for modal content");
            var builder = new StringBuilder(modalFileText);

            builder.Replace("[IframeSource]", secure3dsUrl);

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
            stringBuilder.Append($"elemDiv.innerHTML='{modalContent.Trim()}';");

            stringBuilder.Append("var script = document.createElement('script');");

            var modalJavaScriptContent = GetModalJavaScript();
            stringBuilder.Append($"script.innerHTML='{modalJavaScriptContent.Trim()}';");

            stringBuilder.Append("elemDiv.appendChild(script);");
            stringBuilder.Append("window.parent.document.body.appendChild(elemDiv);</script>");
            stringBuilder.Append(closeIframe);

            return stringBuilder;
        }
        #endregion
    }
}
