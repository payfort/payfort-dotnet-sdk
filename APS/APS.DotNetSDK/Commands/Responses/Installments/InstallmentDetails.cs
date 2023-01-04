using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses.Installments
{
    public class InstallmentDetails
    {
        [JsonPropertyName("issuer_detail")]
        public List<InstallmentsIssuerDetail> InstallmentsIssuerDetail { get; set; }
    }
}
