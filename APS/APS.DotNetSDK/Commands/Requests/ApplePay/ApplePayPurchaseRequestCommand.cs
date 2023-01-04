using System;
using APS.DotNetSDK.Configuration;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayPurchaseRequestCommand : PurchaseRequestCommand
    {
        public ApplePayPurchaseRequestCommand(PurchaseRequestCommand purchaseRequestCommand, 
            ApplePayRequestCommand applePayRequestCommand)
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

            AccessCode = SdkConfiguration.ApplePayConfiguration.AccessCode;
            MerchantIdentifier = SdkConfiguration.ApplePayConfiguration.MerchantIdentifier;
            Language = purchaseRequestCommand.Language;
            MerchantReference = purchaseRequestCommand.MerchantReference;
            Amount = purchaseRequestCommand.Amount;
            Currency = purchaseRequestCommand.Currency;
            CustomerEmail = purchaseRequestCommand.CustomerEmail;
            TokenName = purchaseRequestCommand.TokenName;
            SadadOlp = purchaseRequestCommand.SadadOlp;
            SecurityCode = purchaseRequestCommand.SecurityCode;
            PaymentOption = purchaseRequestCommand.PaymentOption;
            Eci = purchaseRequestCommand.Eci;
            Description = purchaseRequestCommand.Description;
            CustomerIp = purchaseRequestCommand.CustomerIp;
            CustomerName = purchaseRequestCommand.CustomerName;
            MerchantExtra = purchaseRequestCommand.MerchantExtra;
            MerchantExtra1 = purchaseRequestCommand.MerchantExtra1;
            MerchantExtra2 = purchaseRequestCommand.MerchantExtra2;
            MerchantExtra3 = purchaseRequestCommand.MerchantExtra3;
            MerchantExtra4 = purchaseRequestCommand.MerchantExtra4;
            MerchantExtra5 = purchaseRequestCommand.MerchantExtra5;
            RememberMe = purchaseRequestCommand.RememberMe;
            PhoneNumber = purchaseRequestCommand.PhoneNumber;
            SettlementReference = purchaseRequestCommand.SettlementReference;
            ReturnUrl = purchaseRequestCommand.ReturnUrl;
            BillingStateProvince = purchaseRequestCommand.BillingStateProvince;
            BillingProvinceCode = purchaseRequestCommand.BillingProvinceCode;
            BillingStreet = purchaseRequestCommand.BillingStreet;
            BillingStreet2 = purchaseRequestCommand.BillingStreet2;
            BillingPostCode = purchaseRequestCommand.BillingPostCode;
            BillingCountry = purchaseRequestCommand.BillingCountry;
            BillingCompany = purchaseRequestCommand.BillingCompany;
            BillingCity = purchaseRequestCommand.BillingCity;
            ShippingStateProvince = purchaseRequestCommand.ShippingStateProvince;
            ShippingProvinceCode = purchaseRequestCommand.ShippingProvinceCode;
            ShippingStreet = purchaseRequestCommand.ShippingStreet;
            ShippingStreet2 = purchaseRequestCommand.ShippingStreet2;
            ShippingSource = purchaseRequestCommand.ShippingSource;
            ShippingSameAsBilling = purchaseRequestCommand.ShippingSameAsBilling;
            ShippingPostCode = purchaseRequestCommand.ShippingPostCode;
            ShippingCountry = purchaseRequestCommand.ShippingCountry;
            ShippingCompany = purchaseRequestCommand.ShippingCompany;
            ShippingCity = purchaseRequestCommand.ShippingCity;
            AgreementId = purchaseRequestCommand.AgreementId;
            RecurringMode = purchaseRequestCommand.RecurringMode;
            RecurringTransactionsCount = purchaseRequestCommand.RecurringTransactionsCount;
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