using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayPaymentHeader
    {
        [JsonPropertyName("publicKeyHash")]
        public string PublicKeyHash { get; set; }

        [JsonPropertyName("ephemeralPublicKey")]
        public string EphemeralPublicKey { get; set; }

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }
    }
}
