using System;
using APS.DotNetSDK.Configuration;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayAuthorizeRequestCommand : AuthorizeRequestCommand
    {
        public ApplePayAuthorizeRequestCommand(AuthorizeRequestCommand authorizeRequestCommand,
            ApplePayRequestCommand applePayRequestCommand,SdkConfigurationDto account)
        {
            applePayRequestCommand.ValidateMandatoryProperties();

            AppleData = applePayRequestCommand.Data.PaymentData.Data;

            Header = new ApplePayHeader
            {
                EphemeralPublicKey = applePayRequestCommand.Data.PaymentData.Header.EphemeralPublicKey,
                PublicKeyHash = applePayRequestCommand.Data.PaymentData.Header.PublicKeyHash,
                TransactionId = applePayRequestCommand.Data.PaymentData.Header.TransactionId
            };

            PaymentMethod = new Commands.ApplePayPaymentMethod
            {
                DisplayName = applePayRequestCommand.Data.PaymentMethod.DisplayName,
                Network = applePayRequestCommand.Data.PaymentMethod.Network,
                Type = applePayRequestCommand.Data.PaymentMethod.Type,
            };

            AppleSignature = applePayRequestCommand.Data.PaymentData.Signature;

            AccessCode = account.ApplePayConfiguration.AccessCode;
            MerchantIdentifier = account.ApplePayConfiguration.MerchantIdentifier;
            Language = authorizeRequestCommand.Language;
            MerchantReference = authorizeRequestCommand.MerchantReference;
            Amount = authorizeRequestCommand.Amount;
            Currency = authorizeRequestCommand.Currency;
            CustomerEmail = authorizeRequestCommand.CustomerEmail;
            TokenName = authorizeRequestCommand.TokenName;
            SadadOlp = authorizeRequestCommand.SadadOlp;
            SecurityCode = authorizeRequestCommand.SecurityCode;
            PaymentOption = authorizeRequestCommand.PaymentOption;
            Eci = authorizeRequestCommand.Eci;
            Description = authorizeRequestCommand.Description;
            CustomerIp = authorizeRequestCommand.CustomerIp;
            CustomerName = authorizeRequestCommand.CustomerName;
            MerchantExtra = authorizeRequestCommand.MerchantExtra;
            MerchantExtra1 = authorizeRequestCommand.MerchantExtra1;
            MerchantExtra2 = authorizeRequestCommand.MerchantExtra2;
            MerchantExtra3 = authorizeRequestCommand.MerchantExtra3;
            MerchantExtra4 = authorizeRequestCommand.MerchantExtra4;
            MerchantExtra5 = authorizeRequestCommand.MerchantExtra5;
            RememberMe = authorizeRequestCommand.RememberMe;
            PhoneNumber = authorizeRequestCommand.PhoneNumber;
            SettlementReference = authorizeRequestCommand.SettlementReference;
            ReturnUrl = authorizeRequestCommand.ReturnUrl;
            BillingStateProvince = authorizeRequestCommand.BillingStateProvince;
            BillingProvinceCode = authorizeRequestCommand.BillingProvinceCode;
            BillingStreet = authorizeRequestCommand.BillingStreet;
            BillingStreet2 = authorizeRequestCommand.BillingStreet2;
            BillingPostCode = authorizeRequestCommand.BillingPostCode;
            BillingCountry = authorizeRequestCommand.BillingCountry;
            BillingCompany = authorizeRequestCommand.BillingCompany;
            BillingCity = authorizeRequestCommand.BillingCity;
            ShippingStateProvince = authorizeRequestCommand.ShippingStateProvince;
            ShippingProvinceCode = authorizeRequestCommand.ShippingProvinceCode;
            ShippingStreet = authorizeRequestCommand.ShippingStreet;
            ShippingStreet2 = authorizeRequestCommand.ShippingStreet2;
            ShippingSource = authorizeRequestCommand.ShippingSource;
            ShippingSameAsBilling = authorizeRequestCommand.ShippingSameAsBilling;
            ShippingPostCode = authorizeRequestCommand.ShippingPostCode;
            ShippingCountry = authorizeRequestCommand.ShippingCountry;
            ShippingCompany = authorizeRequestCommand.ShippingCompany;
            ShippingCity = authorizeRequestCommand.ShippingCity;
            AgreementId = authorizeRequestCommand.AgreementId;
            RecurringMode = authorizeRequestCommand.RecurringMode;
            RecurringTransactionsCount = authorizeRequestCommand.RecurringTransactionsCount;
        }

        [JsonPropertyName("apple_data")]
        public string AppleData { get; set; }

        [JsonPropertyName("apple_header")]
        public ApplePayHeader Header { get; set; }

        [JsonPropertyName("apple_paymentMethod")]
        public Commands.ApplePayPaymentMethod PaymentMethod { get; set; }

        [JsonPropertyName("apple_signature")]
        public string AppleSignature { get; set; }

        [JsonPropertyName("digital_wallet")]
        public string DigitalWallet => "APPLE_PAY";

        public override void ValidateMandatoryProperties()
        {
            base.ValidateMandatoryProperties();

            if (string.IsNullOrEmpty(AppleData))
            {
                throw new ArgumentNullException($"AppleData", "AppleData is mandatory");
            }

            if (string.IsNullOrEmpty(Header.EphemeralPublicKey))
            {
                throw new ArgumentNullException($"Header.EphemeralPublicKey",
                    "ApplePayHeader.EphemeralPublicKey is mandatory");
            }

            if (string.IsNullOrEmpty(Header.PublicKeyHash))
            {
                throw new ArgumentNullException($"Header.PublicKeyHash",
                    "ApplePayHeader.PublicKeyHash is mandatory");
            }

            if (string.IsNullOrEmpty(Header.TransactionId))
            {
                throw new ArgumentNullException($"Header.TransactionId",
                    "ApplePayHeader.TransactionId is mandatory");
            }

            if (string.IsNullOrEmpty(PaymentMethod.DisplayName))
            {
                throw new ArgumentNullException($"PaymentMethod.DisplayName",
                    "ApplePayPaymentMethod.DisplayName is mandatory");
            }

            if (string.IsNullOrEmpty(PaymentMethod.Type))
            {
                throw new ArgumentNullException($"PaymentMethod.Type",
                    "ApplePayPaymentMethod.Type is mandatory");
            }

            if (string.IsNullOrEmpty(PaymentMethod.Network))
            {
                throw new ArgumentNullException($"PaymentMethod.Network",
                    "ApplePayPaymentMethod.Network is mandatory");
            }

            if (string.IsNullOrEmpty(AppleSignature))
            {
                throw new ArgumentNullException($"AppleSignature",
                    "AppleSignature is mandatory");
            }
        }
    }
}