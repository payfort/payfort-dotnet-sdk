using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands
{
    public class ApplePayHeader
    {
        [JsonPropertyName("apple_ephemeralPublicKey")]
        public string EphemeralPublicKey { get; set; }

        [JsonPropertyName("apple_publicKeyHash")]
        public string PublicKeyHash { get; set; }

        [JsonPropertyName("apple_transactionId")]
        public string TransactionId { get; set; }
    }
}
