using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses.ApplePay
{
    public class ApplePayAuthorizeResponseCommand : AuthorizeResponseCommand
    {
        [JsonPropertyName("apple_data")]
        public string AppleData { get; set; }

        [JsonPropertyName("apple_header")]
        public ApplePayHeader Header { get; set; }

        [JsonPropertyName("apple_paymentMethod")]
        public ApplePayPaymentMethod PaymentMethod { get; set; }

        [JsonPropertyName("apple_signature")]
        public string AppleSignature { get; set; }

        [JsonPropertyName("digital_wallet")]
        public string DigitalWallet { get; set; }
    }
}
