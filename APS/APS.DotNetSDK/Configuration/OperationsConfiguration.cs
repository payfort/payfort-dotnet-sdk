using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class OperationsConfiguration
    {
        [JsonPropertyName("BaseUrl")]
        public string BaseUrl { get; set; }

        [JsonPropertyName("RequestUri")]
        public string RequestUri { get; set; }
    }
}