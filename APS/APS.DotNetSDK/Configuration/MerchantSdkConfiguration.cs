using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class MerchantSdkConfiguration
    {
        [JsonPropertyName("SdkConfiguration")]
        public SdkJsonConfiguration SdkConfiguration { get; set; }

        [JsonPropertyName("SubAccounts")]
        public List<SdkJsonConfiguration> SubAccountsList { get; set; }
    }
}
