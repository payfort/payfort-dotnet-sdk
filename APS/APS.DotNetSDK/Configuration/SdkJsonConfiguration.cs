using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class SdkJsonConfiguration
    {
        [JsonPropertyName("Environment")]
        public string IsTestEnvironmentAsString { get; set; }

        [JsonPropertyName("AccessCode")]
        public string AccessCode { get; set; }

        [JsonPropertyName("MerchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonPropertyName("ResponseShaPhrase")]
        public string ResponseShaPhrase { get; set; }

        [JsonPropertyName("RequestShaPhrase")]
        public string RequestShaPhrase { get; set; }

        [JsonPropertyName("ShaType")]
        public string ShaTypeAsString { get; set; }

        [JsonPropertyName("ApplePay")]
        public ApplePayConfiguration ApplePay { get; set; }
    }
}
