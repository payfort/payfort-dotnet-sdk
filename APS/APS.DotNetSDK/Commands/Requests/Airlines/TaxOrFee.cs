using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class TaxOrFee
    {
         
        [JsonPropertyName("tax_or_fee_amount")]
        public string Amount { get; set; }
         
        [JsonPropertyName("tax_or_fee_type")]
        public string TaxOrFeeType { get; set; }
    }
}