using System.Web;
using System.Text.Json;
using APS.Signature.Utils;
using APS.DotNetSDK.Exceptions;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using APS.DotNetSDK.AcquirerResponseMessage;
using APS.DotNetSDK.Utils;

namespace APS.DotNetSDK.Commands.Responses
{
    public class AuthorizeResponseCommand : ResponseCommandWithNotification
    {
        private const string CommandKey = "command";
        private string _remember;

        [JsonPropertyName("command")]
        public override string Command => "AUTHORIZATION";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("token_name")]
        public string TokenName { get; set; }

        [JsonPropertyName("payment_option")]
        public string PaymentOption { get; set; }
        /// <summary>
        /// This is for redirection
        /// </summary>
        [JsonPropertyName("sadad_olp")]
        public string SadadOlp { get; set; }
        /// <summary>
        /// This is for redirection
        /// </summary>
        [JsonPropertyName("knet_ref_number")]
        public string KnetRefNumber { get; set; }
        /// <summary>
        /// This is for redirection
        /// </summary>
        [JsonPropertyName("third_party_transaction_number")]
        public string ThirdPartyTransactionNumber { get; set; }

        [JsonPropertyName("eci")]
        public string Eci { get; set; }

        [JsonPropertyName("order_description")]
        public string Description { get; set; }

        [JsonPropertyName("customer_ip")]
        public string CustomerIp { get; set; }

        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }

        [JsonPropertyName("merchant_extra")]
        public string MerchantExtra { get; set; }

        [JsonPropertyName("merchant_extra1")]
        public string MerchantExtra1 { get; set; }

        [JsonPropertyName("merchant_extra2")]
        public string MerchantExtra2 { get; set; }

        [JsonPropertyName("merchant_extra3")]
        public string MerchantExtra3 { get; set; }

        [JsonPropertyName("merchant_extra4")]
        public string MerchantExtra4 { get; set; }

        [JsonPropertyName("merchant_extra5")]
        public string MerchantExtra5 { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("card_holder_name")]
        public string CardHolderName { get; set; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }

        /// <summary>
        /// This is for standard and custom checkout
        /// </summary>
        [JsonPropertyName("3ds_url")]
        public string Secure3dsUrl { get; set; }

        /// <summary>
        /// Should be "YES" or "NO"
        /// </summary>
        [JsonPropertyName("remember_me")]
        public string RememberMe
        {
            get => _remember?.ToUpper();
            set => _remember = value;
        }

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("settlement_reference")]
        public string SettlementReference { get; set; }

        [JsonPropertyName("billing_stateProvince")]
        public string BillingStateProvince { get; set; }

        [JsonPropertyName("billing_provinceCode")]
        public string BillingProvinceCode { get; set; }

        [JsonPropertyName("billing_street")]
        public string BillingStreet { get; set; }

        [JsonPropertyName("billing_street2")]
        public string BillingStreet2 { get; set; }

        [JsonPropertyName("billing_postcode")]
        public string BillingPostCode { get; set; }

        [JsonPropertyName("billing_country")]
        public string BillingCountry { get; set; }

        [JsonPropertyName("billing_company")]
        public string BillingCompany { get; set; }

        [JsonPropertyName("billing_city")]
        public string BillingCity { get; set; }

        [JsonPropertyName("shipping_stateProvince")]
        public string ShippingStateProvince { get; set; }

        [JsonPropertyName("shipping_provinceCode")]
        public string ShippingProvinceCode { get; set; }

        [JsonPropertyName("shipping_street")]
        public string ShippingStreet { get; set; }

        [JsonPropertyName("shipping_street2")]
        public string ShippingStreet2 { get; set; }

        [JsonPropertyName("shipping_source")]
        public string ShippingSource { get; set; }

        [JsonPropertyName("shipping_sameAsBilling")]
        public string ShippingSameAsBilling { get; set; }

        [JsonPropertyName("shipping_postcode")]
        public string ShippingPostCode { get; set; }

        [JsonPropertyName("shipping_country")]
        public string ShippingCountry { get; set; }

        [JsonPropertyName("shipping_company")]
        public string ShippingCompany { get; set; }

        [JsonPropertyName("shipping_city")]
        public string ShippingCity { get; set; }

        [JsonPropertyName("app_programming")]
        public string AppProgramming { get; set; }

        [JsonPropertyName("app_plugin")]
        public string AppPlugin { get; set; }

        [JsonPropertyName("app_plugin_version")]
        public string AppPluginVersion { get; set; }

        [JsonPropertyName("app_ver")]
        public string AppVersion { get; set; }

        [JsonPropertyName("app_framework")]
        public string AppFramework { get; set; }

        [JsonPropertyName("agreement_id")]
        public string AgreementId { get; set; }

        [JsonPropertyName("recurring_mode")]
        public string RecurringMode { get; set; }

        [JsonPropertyName("recurring_transactions_count")]
        public string RecurringTransactionsCount { get; set; }

        [JsonPropertyName("acquirer_response_code")]
        public string AcquirerResponseCode { get; set; }        
        
        [JsonPropertyName("acquirer_response_message")]
        public string AcquirerResponseMessage { get; set; }

        [JsonPropertyName("fraud_comment")]
        public string FraudComment { get; set; }

        [IgnoreOnSignatureCalculation(true)]
        [JsonPropertyName("acquirer_response_description")]
        public string AcquirerResponseDescription => !string.IsNullOrEmpty(AcquirerResponseCode) ? 
            new AcquirerResponseMapping().GetAcquirerResponseDescription(AcquirerResponseCode) : null;

        public override void BuildNotificationCommand(IDictionary<string, string> dictionaryObject)
        {
            if (!dictionaryObject.Keys.Contains(CommandKey))
            {
                throw new InvalidNotification(
                    "Response does not contain any command. Please contact the SDK provider.");
            }

            var responseCommand = dictionaryObject.CreateObject<AuthorizeResponseCommand>();

            if (responseCommand == null)
            {
                throw new InvalidNotification(
                    $"There was an issue when mapping notification request: [" +
                    $"{dictionaryObject.JoinElements(",")}] to AuthorizeResponseCommand");
            }

            if (dictionaryObject["command"] != Command)
            {
                throw new InvalidNotification(
                    $"Invalid Command received from payment gateway:{dictionaryObject["command"]}");
            }

            Signature = responseCommand.Signature;

            AccessCode = responseCommand.AccessCode;
            MerchantIdentifier = responseCommand.MerchantIdentifier;
            Language = responseCommand.Language;
            MerchantReference = responseCommand.MerchantReference;
            FortId = responseCommand.FortId;
            ResponseMessage = responseCommand.ResponseMessage;
            ResponseCode = responseCommand.ResponseCode;
            Status = responseCommand.Status;
            AcquirerResponseCode = responseCommand.AcquirerResponseCode;
            AcquirerResponseMessage = responseCommand.AcquirerResponseMessage;
            ReconciliationReference = responseCommand.ReconciliationReference;
            ProcessorResponseCode = responseCommand.ProcessorResponseCode;

            Amount = responseCommand.Amount;
            Currency = responseCommand.Currency;
            CustomerEmail = responseCommand.CustomerEmail;
            TokenName = responseCommand.TokenName;
            PaymentOption = responseCommand.PaymentOption;
            SadadOlp = responseCommand.SadadOlp;
            KnetRefNumber = responseCommand.KnetRefNumber;
            ThirdPartyTransactionNumber = responseCommand.ThirdPartyTransactionNumber;
            Eci = responseCommand.Eci;
            Description = responseCommand.Description;
            CustomerIp = responseCommand.CustomerIp;
            CustomerName = responseCommand.CustomerName;
            MerchantExtra = responseCommand.MerchantExtra;
            MerchantExtra1 = responseCommand.MerchantExtra1;
            MerchantExtra2 = responseCommand.MerchantExtra2;
            MerchantExtra3 = responseCommand.MerchantExtra3;
            MerchantExtra4 = responseCommand.MerchantExtra4;
            MerchantExtra5 = responseCommand.MerchantExtra5;
            AuthorizationCode = responseCommand.AuthorizationCode;
            CardHolderName = responseCommand.CardHolderName;
            ExpiryDate = responseCommand.ExpiryDate;
            CardNumber = responseCommand.CardNumber;
            Secure3dsUrl = responseCommand.Secure3dsUrl;
            RememberMe = responseCommand.RememberMe;
            ReturnUrl = HttpUtility.UrlDecode(responseCommand.ReturnUrl);
            PhoneNumber = responseCommand.PhoneNumber;
            SettlementReference = responseCommand.SettlementReference;
            BillingStateProvince = responseCommand.BillingStateProvince;
            BillingProvinceCode = responseCommand.BillingProvinceCode;
            BillingStreet = responseCommand.BillingStreet;
            BillingStreet2 = responseCommand.BillingStreet2;
            BillingPostCode = responseCommand.BillingPostCode;
            BillingCountry = responseCommand.BillingCountry;
            BillingCompany = responseCommand.BillingCompany;
            BillingCity = responseCommand.BillingCity;
            ShippingStateProvince = responseCommand.ShippingStateProvince;
            ShippingProvinceCode = responseCommand.ShippingProvinceCode;
            ShippingStreet = responseCommand.ShippingStreet;
            ShippingStreet2 = responseCommand.ShippingStreet2;
            ShippingSource = responseCommand.ShippingSource;
            ShippingSameAsBilling = responseCommand.ShippingSameAsBilling;
            ShippingPostCode = responseCommand.ShippingPostCode;
            ShippingCountry = responseCommand.ShippingCountry;
            ShippingCompany = responseCommand.ShippingCompany;
            ShippingCity = responseCommand.ShippingCity;

            AgreementId = responseCommand.AgreementId;
            RecurringMode = responseCommand.RecurringMode;
            RecurringTransactionsCount = responseCommand.RecurringTransactionsCount;

            FraudComment = responseCommand.FraudComment;
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new AuthorizeResponseCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                ResponseCode = this.ResponseCode,
                ResponseMessage = this.ResponseMessage,
                Status = this.Status,
                AcquirerResponseCode = this.AcquirerResponseCode,
                AcquirerResponseMessage = this.AcquirerResponseMessage,
                ProcessorResponseCode = this.ProcessorResponseCode,
                ReconciliationReference = this.ReconciliationReference,

                MerchantReference = this.MerchantReference,
                Amount = this.Amount,
                Currency = this.Currency,
                CustomerEmail = string.IsNullOrEmpty(this.CustomerEmail) ? this.CustomerEmail : "***",
                TokenName = this.TokenName,
                SadadOlp = this.SadadOlp,
                PaymentOption = this.PaymentOption,
                Eci = this.Eci,
                Description = this.Description,
                CustomerIp = string.IsNullOrEmpty(this.CustomerIp) ? this.CustomerIp : "***",
                CustomerName = this.CustomerName,
                MerchantExtra = this.MerchantExtra,
                MerchantExtra1 = this.MerchantExtra1,
                MerchantExtra2 = this.MerchantExtra2,
                MerchantExtra3 = this.MerchantExtra3,
                MerchantExtra4 = this.MerchantExtra4,
                MerchantExtra5 = this.MerchantExtra5,
                RememberMe = this.RememberMe,
                PhoneNumber = this.PhoneNumber,
                SettlementReference = this.SettlementReference,
                ReturnUrl = this.ReturnUrl,
                BillingStateProvince = this.BillingStateProvince,
                BillingProvinceCode = this.BillingProvinceCode,
                BillingStreet = this.BillingStreet,
                BillingStreet2 = this.BillingStreet2,
                BillingPostCode = this.BillingPostCode,
                BillingCountry = this.BillingCountry,
                BillingCompany = this.BillingCompany,
                BillingCity = this.BillingCity,
                ShippingStateProvince = this.ShippingStateProvince,
                ShippingProvinceCode = this.ShippingProvinceCode,
                ShippingStreet = this.ShippingStreet,
                ShippingStreet2 = this.ShippingStreet2,
                ShippingSource = this.ShippingSource,
                ShippingSameAsBilling = this.ShippingSameAsBilling,
                ShippingPostCode = this.ShippingPostCode,
                ShippingCountry = this.ShippingCountry,
                ShippingCompany = this.ShippingCompany,
                ShippingCity = this.ShippingCity,

                FortId = this.FortId,
                KnetRefNumber = this.KnetRefNumber,
                ThirdPartyTransactionNumber = this.ThirdPartyTransactionNumber,
                AuthorizationCode = this.AuthorizationCode,
                CardHolderName = string.IsNullOrEmpty(this.CardHolderName) ? this.CardHolderName : "***",
                CardNumber = string.IsNullOrEmpty(this.CardNumber) ? this.CardNumber : "***",
                ExpiryDate = string.IsNullOrEmpty(this.ExpiryDate) ? this.ExpiryDate : "***",
                Secure3dsUrl = this.Secure3dsUrl,

                AgreementId = this.AgreementId,
                RecurringMode = this.RecurringMode,
                RecurringTransactionsCount = this.RecurringTransactionsCount,
                FraudComment = this.FraudComment
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