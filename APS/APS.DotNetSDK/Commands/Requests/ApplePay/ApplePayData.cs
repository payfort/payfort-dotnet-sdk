using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.ApplePay
{
    public class ApplePayData
    {
        [JsonPropertyName("paymentData")]
        public ApplePayPaymentData PaymentData { get; set; }

        [JsonPropertyName("paymentMethod")]
        public ApplePayPaymentMethod PaymentMethod { get; set; }

        [JsonPropertyName("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }
    }
}
