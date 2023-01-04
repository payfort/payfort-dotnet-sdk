using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands
{
    public class ApplePayPaymentMethod
    {
        [JsonPropertyName("apple_displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("apple_network")]
        public string Network { get; set; }

        [JsonPropertyName("apple_type")]
        public string Type { get; set; }
    }
}
