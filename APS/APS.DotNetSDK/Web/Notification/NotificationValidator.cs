using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using APS.DotNetSDK.Configuration;
using Microsoft.Extensions.Logging;
using APS.DotNetSDK.Commands.Responses;
using APS.DotNetSDK.AcquirerResponseMessage;
using APS.Signature;

namespace APS.DotNetSDK.Web.Notification
{
    public class NotificationValidator : INotificationValidator
    {
        private readonly ILogger<NotificationValidator> _logger;
        private readonly AcquirerResponseMapping _acquirerResponseMapping;
        private readonly ISignatureValidator _signatureValidator;

        private static readonly string[] CommandKeys = new[] { "command", "service_command" };

        internal NotificationValidator(ISignatureValidator signatureValidator)
        {
            SdkConfiguration.Validate();

            _signatureValidator = signatureValidator;
            _acquirerResponseMapping = new AcquirerResponseMapping();

            _logger = SdkConfiguration.LoggerFactory.CreateLogger<NotificationValidator>();
        }

        /// <summary>
        /// Constructor for notification validator
        /// </summary>
        /// <exception cref="Exceptions.SdkConfigurationException">Get the exception when sdk configuration is not set</exception>
        public NotificationValidator() : this(new SignatureValidator())
        {
        }

        public NotificationValidationResponse Validate(HttpRequest httpRequest, string accountName = null)
        {
            var account = SdkConfiguration.GetAccount(accountName);
            switch (httpRequest.Method)
            {
                case "POST":
                    return ValidateFormPost(httpRequest.Form, account);
                case "GET":
                    return ValidateQueryString(httpRequest.QueryString, account);
                default:
                    throw new InvalidNotification("Notification should be GET or POST");
            }
        }

        public NotificationValidationResponse ValidateAsyncNotification(HttpRequest httpRequest, string accountName = null)
        {
            var account = SdkConfiguration.GetAccount(accountName);
            var stream = new StreamReader(httpRequest.Body);
            var body = stream.ReadToEndAsync().Result;

            var keyValuePairDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            keyValuePairDictionary = (Dictionary<string, string>)CheckAcquirerResponseCode(keyValuePairDictionary);

            _logger.LogDebug(
                $"Started to validate the async notification received from payment gateway:[{@ToAnonymized(keyValuePairDictionary)}]");

            var isValid = ValidateRequest(keyValuePairDictionary, account);

            _logger.LogDebug($"Validation is {isValid} for validate request");

            return new NotificationValidationResponse
            {
                IsValid = isValid,
                RequestData = keyValuePairDictionary
            };
        }

        #region  private methods
        private NotificationValidationResponse ValidateFormPost(IFormCollection formCollection, SdkConfigurationDto account)
        {
            var keyValuePairDictionary =
                formCollection.Keys.ToDictionary<string, string, string>(key =>
                    key, key => formCollection[key]);

            keyValuePairDictionary = (Dictionary<string, string>)CheckAcquirerResponseCode(keyValuePairDictionary);

            _logger.LogDebug(
                $"Started to validate the Form Post notification received from payment gateway:[{@ToAnonymized(keyValuePairDictionary)}]");

            var isValid = ValidateRequest(keyValuePairDictionary, account);

            _logger.LogDebug($"Validation is {isValid} for validate request");

            return new NotificationValidationResponse
            {
                IsValid = isValid,
                RequestData = keyValuePairDictionary
            };
        }

        private NotificationValidationResponse ValidateQueryString(QueryString queryString, SdkConfigurationDto account)
        {
            if (!queryString.HasValue)
            {
                _logger.LogError("HttpRequest query string has no value.");
                throw new InvalidNotification("HttpRequest query string has no value");
            }

            var keyValuePairDictionary = ParseQueryString(queryString);

            keyValuePairDictionary = CheckAcquirerResponseCode(keyValuePairDictionary);

            _logger.LogDebug($"Started to validate the Get notification received from payment gateway:[{@ToAnonymized(keyValuePairDictionary)}]");

            var isValid = ValidateRequest(keyValuePairDictionary, account);

            _logger.LogDebug($"Validation is {isValid} for validate request");

            return new NotificationValidationResponse
            {
                IsValid = isValid,
                RequestData = keyValuePairDictionary
            };
        }

        private IDictionary<string, string> CheckAcquirerResponseCode(IDictionary<string, string> keyValuePairDictionary)
        {
            if (keyValuePairDictionary.ContainsKey("acquirer_response_code"))
            {
                if (!keyValuePairDictionary.ContainsKey("acquirer_response_description"))
                {
                    keyValuePairDictionary.Add("acquirer_response_description",
                        _acquirerResponseMapping.GetAcquirerResponseDescription(keyValuePairDictionary["acquirer_response_code"]));
                }
            }

            return keyValuePairDictionary;
        }
        private static IDictionary<string, string> ParseQueryString(QueryString queryString)
        {
            var collection = HttpUtility.ParseQueryString(queryString.Value);
            return collection.AllKeys.ToDictionary(k => k, k => collection[k]);
        }

        private string ToAnonymized(IDictionary<string, string> dictionary)
        {
            var anonymizedDictionary = new Dictionary<string, string>();

            var keysToAnonymize = new List<string>()
            {
                "customer_email", "customer_name", "customer_ip", "card_number",
                "card_holder_name", "card_security_code", "expiry_date"
            };
            foreach (var item in dictionary)
            {
                if (keysToAnonymize.Any(x => x == item.Key))
                {
                    anonymizedDictionary.Add(item.Key, "***");
                    continue;
                }

                anonymizedDictionary.Add(item.Key, item.Value);
            }

            return anonymizedDictionary.JoinElements(",");
        }

        private bool ValidateRequest(IDictionary<string, string> keyValuePairDictionary, SdkConfigurationDto account)
        {
            var responseCommand = BuildResponse(keyValuePairDictionary);

            return ValidateResponseSignature(responseCommand, account);
        }

        private ResponseCommandWithNotification BuildResponse(IDictionary<string, string> keyValuePairDictionary)
        {
            _logger.LogInformation($"Started to build the received notification: [{@ToAnonymized(keyValuePairDictionary)}]");

            ResponseCommandWithNotification responseCommand = BuildResponseCommand(keyValuePairDictionary);

            _logger.LogInformation(
                $"Object response received from the gateway for:[MerchantReference:{responseCommand.MerchantReference},FortId:{responseCommand.FortId}]");

            _logger.LogDebug($"Object response received from the gateway for:[MerchantReference:{responseCommand.MerchantReference}" +
                $",Response:{@responseCommand.ToAnonymizedJson()}]");

            _logger.LogInformation(
                $"Successfully build the received notification for: [MerchantReference:{responseCommand.MerchantReference},FortId:{responseCommand.FortId}]");

            return responseCommand;
        }

        private ResponseCommandWithNotification BuildResponseCommand(IDictionary<string, string> keyValuePairDictionary)
        {
            var type = typeof(ResponseCommandWithNotification);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(y => type.IsAssignableFrom(y) && type.AssemblyQualifiedName != y.AssemblyQualifiedName);

            foreach (var itemType in types)
            {
                var responseCommandItem = (ResponseCommandWithNotification)Activator.CreateInstance(itemType);

                string commandName = GetCommandName(keyValuePairDictionary);
                if (responseCommandItem.Command == commandName)
                {
                    responseCommandItem.BuildNotification(keyValuePairDictionary);
                    return responseCommandItem;
                }
            }

            var message = $"Invalid notification with [{@ToAnonymized(keyValuePairDictionary)}]";
            _logger.LogError(message);

            throw new InvalidNotification(message);
        }

        private static string GetCommandName(IDictionary<string, string> keyValuePairDictionary)
        {
            foreach (var command in CommandKeys)
            {
                if (keyValuePairDictionary.ContainsKey(command))
                {
                    return keyValuePairDictionary[command];
                }
            }

            return null;
        }

        private bool ValidateResponseSignature(ResponseCommandWithNotification responseCommand, SdkConfigurationDto account)
        {
            _logger.LogDebug(
                $"Validate signature for response received from APS [MerchantReference:{responseCommand.MerchantReference},FortId:{responseCommand.FortId}]");

            var isResponseSignatureValid = _signatureValidator.ValidateSignature(responseCommand, account.ResponseShaPhrase, account.ShaType,
                       responseCommand.Signature);

            _logger.LogDebug(
                $"Signature validation is {isResponseSignatureValid} for: [MerchantReference:{responseCommand.MerchantReference},FortId:{responseCommand.FortId}]");

            return isResponseSignatureValid;
        }
        #endregion
    }
}
