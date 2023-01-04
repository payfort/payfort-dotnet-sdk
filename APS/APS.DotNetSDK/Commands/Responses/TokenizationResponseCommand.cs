using System.Web;
using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Exceptions;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses
{
    public class TokenizationResponseCommand : ResponseCommandWithNotification
    {
        private string _rememberMe;
        private const string CommandKey = "service_command";

        [JsonPropertyName("service_command")]
        public override string Command => "TOKENIZATION";

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }

        [JsonPropertyName("token_name")]
        public string TokenName { get; set; }

        [JsonPropertyName("card_bin")]
        public string CardBin { get; set; }

        /// <summary>
        /// This is for custom checkout
        /// </summary>
        [JsonPropertyName("card_holder_name")]
        public string CardHolderName { get; set; }
        /// <summary>
        /// This is only for custom checkout (optional parameter). Should be "YES" or "NO"
        /// </summary>
        [JsonPropertyName("remember_me")]
        public string RememberMe
        {
            get => _rememberMe?.ToUpper();
            set => _rememberMe = value;
        }

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("installments")]
        public string Installments { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("amount")]
        public double? Amount { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("customer_country_code")]
        public string CustomerCountryCode { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("number_of_installments")]
        public string NumberOfInstallments { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("plan_code")]
        public string PlanCode { get; set; }
        /// <summary>
        /// This is only for installments
        /// </summary>
        [JsonPropertyName("issuer_code")]
        public string IssuerCode { get; set; }

        public override void BuildNotificationCommand(IDictionary<string, string> dictionaryObject)
        {
            if (!dictionaryObject.Keys.Contains(CommandKey))
            {
                throw new InvalidNotification(
                    "Response does not contain any command. Please contact the SDK provider.");
            }

            var responseCommand = dictionaryObject.CreateObject<TokenizationResponseCommand>();

            if (responseCommand == null)
            {
                throw new InvalidNotification(
                    $"There was an issue when mapping notification request: [" +
                    $"{dictionaryObject.JoinElements(",")}] to AuthorizeResponseCommand");
            }

            if (dictionaryObject[CommandKey] != Command)
            {
                throw new InvalidNotification(
                    $"Invalid Command received from payment gateway:{dictionaryObject["service_command"]}");
            }

            Signature = responseCommand.Signature;

            AccessCode = responseCommand.AccessCode;
            MerchantIdentifier = responseCommand.MerchantIdentifier;
            Language = responseCommand.Language;
            MerchantReference = responseCommand.MerchantReference;
            ResponseMessage = responseCommand.ResponseMessage;
            ResponseCode = responseCommand.ResponseCode;
            Status = responseCommand.Status; 
            ReconciliationReference = responseCommand.ReconciliationReference;
            ProcessorResponseCode = responseCommand.ProcessorResponseCode;

            ExpiryDate = responseCommand.ExpiryDate;
            CardNumber = responseCommand.CardNumber;
            CardHolderName = responseCommand.CardHolderName;
            CardBin = responseCommand.CardBin;
            TokenName = responseCommand.TokenName;
            ReturnUrl = HttpUtility.UrlDecode(responseCommand.ReturnUrl);
            RememberMe = responseCommand.RememberMe;

            Installments = responseCommand.Installments;
            Amount = responseCommand.Amount;
            Currency = responseCommand.Currency;
            CustomerCountryCode = responseCommand.CustomerCountryCode;
            NumberOfInstallments = responseCommand.NumberOfInstallments;
            PlanCode = responseCommand.PlanCode;
            IssuerCode = responseCommand.IssuerCode;
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new TokenizationResponseCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                ResponseCode = this.ResponseCode,
                ResponseMessage = this.ResponseMessage,
                Status = this.Status,
                ProcessorResponseCode = this.ProcessorResponseCode,
                ReconciliationReference = this.ReconciliationReference,

                MerchantReference = this.MerchantReference,
                ExpiryDate = string.IsNullOrEmpty(ExpiryDate) ? this.ExpiryDate : "***",
                CardNumber = string.IsNullOrEmpty(CardNumber) ? this.CardNumber : "***",
                TokenName = this.TokenName,
                CardBin = string.IsNullOrEmpty(CardBin) ? this.CardBin : "***",
                CardHolderName = string.IsNullOrEmpty(CardHolderName) ? this.CardHolderName : "***",
                RememberMe = this.RememberMe,
                ReturnUrl = this.ReturnUrl, 
                Installments =this.Installments,
                Amount =this.Amount,
                Currency = this.Currency,
                CustomerCountryCode = this.CustomerCountryCode,
                NumberOfInstallments = this.NumberOfInstallments,
                PlanCode = this.PlanCode,
                IssuerCode = this.IssuerCode
            };

            var serialized = JsonSerializer.Serialize(anonymized,
                   new JsonSerializerOptions
                   {
                       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                   });

            return serialized;
        }
    }
}