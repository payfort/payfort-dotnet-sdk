using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Commands.Requests;
using APS.DotNetSDK.Tests.Commands;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Signature.Models
{
    internal class TwoLevelsPropertyRequestCommand : TestRequestCommand
    {
        [JsonPropertyName("payment_type")]
        public Payment? PaymentType { get; set; }

    }

    internal class Payment
    {
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        [JsonPropertyName("payment_cardtype")]
        public string? CardType { get; set; }

        [JsonPropertyName("payment_security_code")]
        public int? SecurityCode { get; set; }
    }
}
