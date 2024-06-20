using APS.Signature.Utils;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands
{
    public class Command
    {
        [IgnoreOnSignatureCalculation(true)]
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
