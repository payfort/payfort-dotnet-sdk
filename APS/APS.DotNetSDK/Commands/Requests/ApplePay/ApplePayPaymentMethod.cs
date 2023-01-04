using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayPaymentMethod
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("network")]
        public string Network { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
