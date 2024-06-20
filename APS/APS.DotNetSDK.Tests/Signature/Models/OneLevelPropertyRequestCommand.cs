using APS.DotNetSDK.Tests.Commands;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Signature.Models
{
    public class OneLevelPropertyRequestCommand : TestRequestCommand
    {
        [JsonPropertyName("bank")]
        public string? ZBank { get; set; }

        [JsonPropertyName("authentication_code")]
        public string? XAuthenticationCode { get; set; }
    }
}
