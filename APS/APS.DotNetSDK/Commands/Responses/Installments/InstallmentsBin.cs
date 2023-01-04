using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses.Installments
{
    public class InstallmentsBin
    {
        [JsonPropertyName("bin")]
        public string Bin { get; set; }

        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("card_brand_code")]
        public string CardBrandCode { get; set; }
    }
}
