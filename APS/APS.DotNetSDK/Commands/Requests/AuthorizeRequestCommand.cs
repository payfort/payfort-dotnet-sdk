using System;
using System.Text.Json;
using System.Reflection;
using System.Text.Encodings.Web;
using APS.DotNetSDK.Configuration;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests
{
    public class AuthorizeRequestCommand : RequestCommand
    {
        private string _rememberMe;

        public AuthorizeRequestCommand()
        {
            AccessCode = SdkConfiguration.AccessCode;
            MerchantIdentifier = SdkConfiguration.MerchantIdentifier;
        }

        [JsonPropertyName("command")]
        public string Command => "AUTHORIZATION";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("token_name")]
        public string TokenName { get; set; }

        /// <summary>
        /// This is for redirection
        /// </summary>
        [JsonPropertyName("sadad_olp")]
        public string SadadOlp { get; set; }

        /// <summary>
        /// This for standard, custom checkout and trusted channels
        /// </summary>
        [JsonPropertyName("card_security_code")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("payment_option")]
        public string PaymentOption { get; set; }

        [JsonPropertyName("eci")]
        public string Eci { get; set; }

        /// <summary>
        /// This is only for trusted channels
        /// </summary>
        [JsonPropertyName("card_holder_name")]
        public string CardHolderName { get; set; }
        /// <summary>
        /// This is only for trusted channels
        /// </summary>
        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }
        /// <summary>
        /// This is only for trusted channels
        /// </summary>
        [JsonPropertyName("card_number")]
        public string CardNumber { get; set; }

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

        /// <summary>
        /// Should be "YES" or "NO"
        /// </summary>
        [JsonPropertyName("remember_me")]
        public string RememberMe
        {
            get => _rememberMe?.ToUpper();
            set => _rememberMe = value;
        }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("settlement_reference")]
        public string SettlementReference { get; set; }

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }

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

        /// <summary>
        /// This is only for recurring
        /// </summary>
        [JsonPropertyName("agreement_id")]
        public string AgreementId { get; set; }

        /// <summary>
        /// This is only for recurring
        /// </summary>
        [JsonPropertyName("recurring_mode")]
        public string RecurringMode { get; set; }

        /// <summary>
        /// This is only for recurring
        /// </summary>
        [JsonPropertyName("recurring_transactions_count")]
        public string RecurringTransactionsCount { get; set; }

        [JsonPropertyName("app_programming")]
        public string AppProgramming => ".NET";

        [JsonPropertyName("app_plugin")]
        public string AppPlugin => ".dotNETSDK";

        [JsonPropertyName("app_plugin_version")]
        public string AppPluginVersion => "v2.0.0";

        [JsonPropertyName("app_ver")]
        public string AppVersion => Assembly.GetAssembly(typeof(RequestCommand)).GetName().Version.ToString();

        [JsonPropertyName("app_framework")]
        public string AppFramework => ".NET";

        [JsonPropertyName("customer_type")]
        public string CustomerType { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        [JsonPropertyName("customer_first_name")]
        public string CustomerFirstName { get; set; }

        [JsonPropertyName("customer_middle_initial")]
        public string CustomerMiddleInitial { get; set; }

        [JsonPropertyName("customer_last_name")]
        public string CustomerLastName { get; set; }

        [JsonPropertyName("customer_address1")]
        public string CustomerAddress1 { get; set; }

        [JsonPropertyName("customer_address2")]
        public string CustomerAddress2 { get; set; }

        [JsonPropertyName("customer_apartment_no")]
        public string CustomerApartmentNo { get; set; }

        [JsonPropertyName("customer_city")]
        public string CustomerCity { get; set; }

        [JsonPropertyName("customer_state")]
        public string CustomerState { get; set; }

        [JsonPropertyName("customer_zip_code")]
        public string CustomerZipCode { get; set; }

        [JsonPropertyName("customer_country_code")]
        public string CustomerCountryCode { get; set; }

        [JsonPropertyName("customer_phone")]
        public string CustomerPhone { get; set; }

        [JsonPropertyName("customer_alt_phone")]
        public string CustomerAltPhone { get; set; }

        [JsonPropertyName("customer_date_birth")]
        public string CustomerDateBirth { get; set; }

        [JsonPropertyName("ship_type")]
        public string ShipType { get; set; }

        [JsonPropertyName("ship_first_name")]
        public string ShipFirstName { get; set; }

        [JsonPropertyName("ship_middle_name")]
        public string ShipMiddleName { get; set; }

        [JsonPropertyName("ship_last_name")]
        public string ShipLastName { get; set; }

        [JsonPropertyName("ship_address1")]
        public string ShipAddress1 { get; set; }

        [JsonPropertyName("ship_address2")]
        public string ShipAddress2 { get; set; }

        [JsonPropertyName("ship_apartment_no")]
        public string ShipApartmentNo { get; set; }

        [JsonPropertyName("ship_address_city")]
        public string ShipAddressCity { get; set; }

        [JsonPropertyName("ship_address_state")]
        public string ShipAddressState { get; set; }

        [JsonPropertyName("ship_zip_code")]
        public string ShipZipCode { get; set; }

        [JsonPropertyName("ship_country_code")]
        public string ShipCountryCode { get; set; }

        [JsonPropertyName("ship_phone")]
        public string ShipPhone { get; set; }

        [JsonPropertyName("ship_alt_phone")]
        public string ShipAltPhone { get; set; }

        [JsonPropertyName("ship_email")]
        public string ShipEmail { get; set; }

        [JsonPropertyName("ship_comments")]
        public string ShipComments { get; set; }

        [JsonPropertyName("ship_method")]
        public string ShipMethod { get; set; }

        [JsonPropertyName("fraud_extra1")]
        public string FraudExtra1 { get; set; }

        [JsonPropertyName("fraud_extra2")]
        public string FraudExtra2 { get; set; }

        [JsonPropertyName("fraud_extra3")]
        public string FraudExtra3 { get; set; }

        [JsonPropertyName("fraud_extra4")]
        public string FraudExtra4 { get; set; }

        [JsonPropertyName("fraud_extra5")]
        public string FraudExtra5 { get; set; }

        [JsonPropertyName("fraud_extra6")]
        public string FraudExtra6 { get; set; }

        [JsonPropertyName("fraud_extra7")]
        public string FraudExtra7 { get; set; }

        [JsonPropertyName("fraud_extra8")]
        public string FraudExtra8 { get; set; }

        [JsonPropertyName("fraud_extra9")]
        public string FraudExtra9 { get; set; }

        [JsonPropertyName("fraud_extra10")]
        public string FraudExtra10 { get; set; }

        [JsonPropertyName("fraud_extra11")]
        public string FraudExtra11 { get; set; }

        [JsonPropertyName("fraud_extra12")]
        public string FraudExtra12 { get; set; }

        [JsonPropertyName("fraud_extra13")]
        public string FraudExtra13 { get; set; }

        [JsonPropertyName("fraud_extra14")]
        public string FraudExtra14 { get; set; }

        [JsonPropertyName("fraud_extra15")]
        public string FraudExtra15 { get; set; }

        [JsonPropertyName("fraud_extra16")]
        public string FraudExtra16 { get; set; }

        [JsonPropertyName("fraud_extra17")]
        public string FraudExtra17 { get; set; }

        [JsonPropertyName("fraud_extra18")]
        public string FraudExtra18 { get; set; }

        [JsonPropertyName("fraud_extra19")]
        public string FraudExtra19 { get; set; }

        [JsonPropertyName("fraud_extra20")]
        public string FraudExtra20 { get; set; }

        [JsonPropertyName("fraud_extra21")]
        public string FraudExtra21 { get; set; }

        [JsonPropertyName("fraud_extra22")]
        public string FraudExtra22 { get; set; }

        [JsonPropertyName("fraud_extra23")]
        public string FraudExtra23 { get; set; }

        [JsonPropertyName("fraud_extra24")]
        public string FraudExtra24 { get; set; }

        [JsonPropertyName("fraud_extra25")]
        public string FraudExtra25 { get; set; }

        [JsonPropertyName("cart_details")]
        public string CartDetails { get; set; }

        [JsonPropertyName("device_fingerprint")]
        public string DeviceFingerPrint { get; set; }

        [JsonPropertyName("item_quantity")]
        public string ItemQuantity { get; set; }

        [JsonPropertyName("item_sku")]
        public string ItemSku { get; set; }

        [JsonPropertyName("item_prod_code")]
        public string ItemProdCode { get; set; }

        [JsonPropertyName("item_part_no")]
        public string ItemPartNo { get; set; }

        [JsonPropertyName("item_description")]
        public string ItemDescription { get; set; }

        [JsonPropertyName("item_price")]
        public double? ItemPrice { get; set; }

        [JsonPropertyName("item_shipping_no")]
        public string ItemShippingNo { get; set; }

        [JsonPropertyName("item_shipping_method")]
        public string ItemShippingMethod { get; set; }

        [JsonPropertyName("item_shipping_comments")]
        public string ItemShippingComments { get; set; }

        [JsonPropertyName("item_gift_msg")]
        public string ItemGiftMsg { get; set; }

        [JsonPropertyName("rcpt_title")]
        public string RcptTitle { get; set; }

        [JsonPropertyName("rcpt_first_name")]
        public string RcptFirstName { get; set; }

        [JsonPropertyName("rcpt_middle_initial")]
        public string RcptMiddleInitial { get; set; }

        [JsonPropertyName("rcpt_last_name")]
        public string RcptLastName { get; set; }

        [JsonPropertyName("rcpt_apartment_no")]
        public string RcptApartmentNo { get; set; }

        [JsonPropertyName("rcpt_address1")]
        public string RcptAddress1 { get; set; }

        [JsonPropertyName("rcpt_address2")]
        public string RcptAddress2 { get; set; }

        [JsonPropertyName("rcpt_city")]
        public string RcptCity { get; set; }

        [JsonPropertyName("rcpt_state")]
        public string RcptState { get; set; }

        [JsonPropertyName("rcpt_zip_code")]
        public string RcptZipCode { get; set; }

        [JsonPropertyName("rcpt_country_code")]
        public string RcptCountryCode { get; set; }

        [JsonPropertyName("rcpt_phone")]
        public string RcptPhone { get; set; }

        [JsonPropertyName("rcpt_email")]
        public string RcptEmail { get; set; }

        public override void ValidateMandatoryProperties()
        {
            base.ValidateMandatoryProperties();

            if (string.IsNullOrEmpty(MerchantReference))
            {
                throw new ArgumentNullException($"MerchantReference", "MerchantReference is mandatory");
            }

            if (Amount <= 0)
            {
                throw new ArgumentNullException($"Amount", "Amount is mandatory");
            }

            if (string.IsNullOrEmpty(Currency))
            {
                throw new ArgumentNullException($"Currency", "Currency is mandatory");
            }

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new ArgumentNullException($"CustomerEmail", "CustomerEmail is mandatory");
            }

            if (!string.IsNullOrEmpty(RememberMe))
            {
                if (!(RememberMe == "YES" || RememberMe == "NO"))
                {
                    throw new ArgumentException("RememberMe should be \"YES\" or \"NO\"", $"RememberMe");
                }
            }

            if (!string.IsNullOrEmpty(Eci))
            {
                if (!(Eci == "MOTO" || Eci == "RECURRING" || Eci == "ECOMMERCE"))
                {
                    throw new ArgumentException("Eci should be \"MOTO\",\"RECURRING\" or \"ECOMMERCE\"", $"Eci");
                }
            }
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new AuthorizeRequestCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                MerchantReference = this.MerchantReference,
                Amount = this.Amount,
                Currency = this.Currency,
                CustomerEmail = string.IsNullOrEmpty(this.CustomerEmail) ? this.CustomerEmail : "***",
                TokenName = this.TokenName,
                SadadOlp = this.SadadOlp,
                SecurityCode = string.IsNullOrEmpty(this.SecurityCode) ? this.SecurityCode : "***",
                PaymentOption = this.PaymentOption,
                Eci = this.Eci,
                Description = this.Description,
                CustomerIp = string.IsNullOrEmpty(this.CustomerIp) ? this.CustomerIp : "***",
                CardNumber = string.IsNullOrEmpty(this.CardNumber) ? this.CardNumber : "***",
                ExpiryDate = string.IsNullOrEmpty(this.ExpiryDate) ? this.ExpiryDate : "***",
                CardHolderName = string.IsNullOrEmpty(this.CardHolderName) ? this.CardHolderName : "***",
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
                AgreementId = this.AgreementId,
                RecurringMode = this.RecurringMode,
                RecurringTransactionsCount = this.RecurringTransactionsCount,

                CustomerType = this.CustomerType,
                CustomerId = this.CustomerId,
                CustomerFirstName = this.CustomerFirstName,
                CustomerMiddleInitial = this.CustomerMiddleInitial,
                CustomerLastName = this.CustomerLastName,
                CustomerApartmentNo = this.CustomerApartmentNo,
                CustomerCity = this.CustomerCity,
                CustomerState = this.CustomerState,
                CustomerZipCode = this.CustomerZipCode,
                CustomerCountryCode = this.CustomerCountryCode,
                CustomerPhone = this.CustomerPhone,
                CustomerAltPhone = this.CustomerAltPhone,
                CustomerDateBirth = this.CustomerDateBirth,
                ShipType = this.ShipType,
                ShipFirstName = this.ShipFirstName,
                ShipMiddleName = this.ShipMiddleName,
                ShipLastName = this.ShipLastName,
                ShipAddress1 = this.ShipAddress1,
                ShipAddress2 = this.ShipAddress2,
                ShipApartmentNo = this.ShipApartmentNo,
                ShipAddressCity = this.ShipAddressCity,
                ShipAddressState = this.ShipAddressState,
                ShipZipCode = this.ShipZipCode,
                ShipCountryCode = this.ShipCountryCode,
                ShipPhone = this.ShipPhone,
                ShipAltPhone = this.ShipAltPhone,
                ShipEmail = this.ShipEmail,
                ShipComments = this.ShipComments,
                ShipMethod = this.ShipMethod,
                FraudExtra1 = this.FraudExtra1,
                FraudExtra2 = this.FraudExtra2,
                FraudExtra3 = this.FraudExtra3,
                FraudExtra4 = this.FraudExtra4,
                FraudExtra5 = this.FraudExtra5,
                FraudExtra6 = this.FraudExtra6,
                FraudExtra7 = this.FraudExtra7,
                FraudExtra8 = this.FraudExtra8,
                FraudExtra9 = this.FraudExtra9,
                FraudExtra10 = this.FraudExtra10,
                FraudExtra11 = this.FraudExtra11,
                FraudExtra12 = this.FraudExtra12,
                FraudExtra13 = this.FraudExtra13,
                FraudExtra14 = this.FraudExtra14,
                FraudExtra15 = this.FraudExtra15,
                FraudExtra16 = this.FraudExtra16,
                FraudExtra17 = this.FraudExtra17,
                FraudExtra18 = this.FraudExtra18,
                FraudExtra19 = this.FraudExtra19,
                FraudExtra20 = this.FraudExtra20,
                FraudExtra21 = this.FraudExtra21,
                FraudExtra22 = this.FraudExtra22,
                FraudExtra23 = this.FraudExtra23,
                FraudExtra24 = this.FraudExtra24,
                FraudExtra25 = this.FraudExtra25,
                CartDetails = this.CartDetails,
                DeviceFingerPrint = this.DeviceFingerPrint,

                ItemQuantity = this.ItemQuantity,
                ItemSku = this.ItemSku,
                ItemProdCode = this.ItemProdCode,
                ItemPartNo = this.ItemPartNo,
                ItemDescription = this.ItemDescription,
                ItemPrice = this.ItemPrice,
                ItemShippingNo = this.ItemShippingNo,
                ItemShippingComments = this.ItemShippingComments,
                ItemGiftMsg = this.ItemGiftMsg,
                RcptTitle = this.RcptTitle,
                RcptFirstName = this.RcptFirstName,
                RcptMiddleInitial = this.RcptMiddleInitial,
                RcptLastName = this.RcptLastName,
                RcptApartmentNo = this.RcptApartmentNo,
                RcptAddress1 = this.RcptAddress1,
                RcptAddress2 = this.RcptAddress2,
                RcptCity = this.RcptCity,
                RcptState = this.RcptState,
                RcptZipCode = this.RcptZipCode,
                RcptCountryCode = this.RcptCountryCode,
                RcptPhone = this.RcptPhone,
                RcptEmail = this.RcptEmail
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