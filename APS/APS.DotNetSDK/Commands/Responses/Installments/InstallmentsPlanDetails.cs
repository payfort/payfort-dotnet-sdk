using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses.Installments
{
    public class InstallmentsPlanDetails
    {
        [JsonPropertyName("plan_code")]
        public string PlanCode { get; set; }

        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("number_of_installment")]
        public int NumberOfInstallment { get; set; }

        [JsonPropertyName("fees_type")]
        public string FeesType { get; set; }
        
        [JsonPropertyName("fees_amount")]
        public double? FeesAmount { get; set; }
        
        [JsonPropertyName("processing_fees_type")]
        public string ProcessingFeesType { get; set; }
        
        [JsonPropertyName("processing_fees_amount")]
        public int? ProcessingFeesAmount { get; set; }

        [JsonPropertyName("rate_type")]
        public string RateType { get; set; }

        [JsonPropertyName("plan_merchant_type")]
        public string PlanMerchantType { get; set; }

        [JsonPropertyName("plan_type")]
        public string PlanType { get; set; }

        [JsonPropertyName("fee_display_value")]
        public double? FeeDisplayValue { get; set; }

        [JsonPropertyName("minimum_amount")]
        public double? MinimumAmount { get; set; }

        [JsonPropertyName("maximum_amount")]
        public double? MaximumAmount { get; set; }

        [JsonPropertyName("amountPerMonth")]
        public double? AmountPerMonth { get; set; }
    }
}
