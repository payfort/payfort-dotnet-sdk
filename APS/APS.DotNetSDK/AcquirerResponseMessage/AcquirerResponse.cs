using System.Text.Json.Serialization;

namespace APS.DotNetSDK.AcquirerResponseMessage
{
    public class AcquirerResponse
    {
        [JsonPropertyName("acquirer_response_code")] 
        public string AcquirerResponseCode { get; set; }

        [JsonPropertyName("acquirer_response_description")] 
        public string AcquirerResponseDescription { get; set; }
    }
}
