using System.Text.Json.Serialization;
using APS.Signature.Utils;

namespace APS.DotNetSDK.Tests.Utils
{
    public class CustomAttributesTest
    {
        [IgnoreOnSignatureCalculation(true)]
        [JsonPropertyName("TestAttibute")]
        public string? TestAttribute { get; set; }

        [JsonPropertyName("TestAttibute2")]
        public string? TestAttribute2 { get; set; }
    }
}
