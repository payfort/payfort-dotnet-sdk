using System;
using System.Security.Authentication;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class ApplePaySessionRetrievalConfiguration
    {
        [JsonPropertyName("sslProtocol")]
        public string SslProtocolAsString { get; set; }

        public SslProtocols SslProtocol => (SslProtocols)Enum.Parse(typeof(SslProtocols), SslProtocolAsString);

        [JsonPropertyName("initiative")]
        public string Initiative { get; set; }

        [JsonPropertyName("initiativeContext")]
        public string InitiativeContext { get; set; }
    }
}