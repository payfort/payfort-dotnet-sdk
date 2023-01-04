using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.AcquirerResponseMessage
{
    public class AcquirerJsonResponseMapping
    {
        [JsonPropertyName("AcquirerResponseMapping")]
        public List<AcquirerResponse> AcquirerResponse { get; set; }
    }
}
