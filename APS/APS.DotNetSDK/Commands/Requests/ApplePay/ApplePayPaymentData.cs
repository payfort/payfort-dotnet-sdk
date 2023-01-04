using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayPaymentData
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("header")]
        public ApplePayPaymentHeader Header { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}