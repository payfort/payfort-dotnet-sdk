using APS.DotNetSDK.Tests.Commands;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Signature.Models
{
    internal class TwoLevelsPropertyRequestCommandWithCollection : TestRequestCommand
    {
        [JsonPropertyName("payment_type")]
        public List<Payment>? PaymentType { get; set; }
    }
}