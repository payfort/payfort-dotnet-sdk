using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses.Installments
{
    public class InstallmentsIssuerDetail
    {
        [JsonPropertyName("issuer_code")]
        public string IssuerCode { get; set; }

        [JsonPropertyName("issuer_name_ar")]
        public string IssuerNameAr { get; set; }

        [JsonPropertyName("issuer_name_en")]
        public string IssuerNameEn { get; set; }

        [JsonPropertyName("terms_and_condition_ar")]
        public string TermsAndConditionAr { get; set; }

        [JsonPropertyName("terms_and_condition_en")]
        public string TermsAndConditionEn { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("issuer_logo_ar")]
        public string IssuerLogoAr { get; set; }

        [JsonPropertyName("issuer_logo_en")]
        public string IssuerLogoEn { get; set; }

        [JsonPropertyName("banking_system")]
        public string BankingSystem { get; set; }

        [JsonPropertyName("formula")]
        public string Formula { get; set; }

        [JsonPropertyName("plan_details")]
        public List<InstallmentsPlanDetails> PlanDetails { get; set; }

        [JsonPropertyName("bins")]
        public List<InstallmentsBin> Bins { get; set; }

        [JsonPropertyName("confirmation_message_ar")]
        public string ConfirmationMessageAr { get; set; }

        [JsonPropertyName("disclaimer_message_ar")]
        public string DisclaimerMessageAr { get; set; }

        [JsonPropertyName("processing_fees_message_ar")]
        public string ProcessingFeesMessageAr { get; set; }

        [JsonPropertyName("confirmation_message_en")]
        public string ConfirmationMessageEn { get; set; }

        [JsonPropertyName("disclaimer_message_en")]
        public string DisclaimerMessageEn { get; set; }

        [JsonPropertyName("processing_fees_message_en")]
        public string ProcessingFeesMessageEn { get; set; }

    }
}
