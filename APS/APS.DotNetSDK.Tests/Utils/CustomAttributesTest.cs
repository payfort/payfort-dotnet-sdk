using APS.DotNetSDK.Utils;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Utils
{
    public class CustomAttributesTest
    {
        [IgnoreOnSignatureCalculationAttribute(true)]
        [JsonPropertyName("TestAttibute")]
        public string? TestAttribute { get; set; }

        [JsonPropertyName("TestAttibute2")]
        public string? TestAttribute2 { get; set; }
    }
}
