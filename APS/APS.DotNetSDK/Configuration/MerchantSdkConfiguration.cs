using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class MerchantSdkConfiguration
    {
        [JsonPropertyName("SdkConfiguration")]
        public SdkJsonConfiguration SdkConfiguration { get; set; }
    }
}
