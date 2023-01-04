using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class ApsJsonConfiguration
    {
        [JsonPropertyName("Environments")]
        public List<ApsEnvironmentConfiguration> Environments { get; set; }

        [JsonPropertyName("ApplePaySessionRetrieval")]
        public ApplePaySessionRetrievalConfiguration ApplePaySessionRetrieval { get; set; }

        [JsonPropertyName("RedirectFormTemplate")]
        public string RedirectFormTemplate { get; set; }

        [JsonPropertyName("StandardFormTemplate")]
        public string StandardFormTemplate { get; set; }

        [JsonPropertyName("CustomFormTemplate")]
        public string CustomFormTemplate { get; set; }

        [JsonPropertyName("CloseModalJavaScript")]
        public string CloseModalJavaScript { get; set; }

        [JsonPropertyName("CloseIframeJavaScript")]
        public string CloseIframeJavaScript { get; set; }

    }
}
