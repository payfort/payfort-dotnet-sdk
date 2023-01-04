using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Configuration
{
    public class ApsEnvironmentConfiguration
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("MaintenanceOperations")]
        public OperationsConfiguration MaintenanceOperations { get; set; }

        [JsonPropertyName("Installments")]
        public OperationsConfiguration Installments { get; set; }

        [JsonPropertyName("RedirectUrl")]
        public string RedirectUrl { get; set; }  
        
        [JsonPropertyName("StandardCheckoutActionUrl")]
        public string StandardCheckoutActionUrl { get; set; } 
        
        [JsonPropertyName("CustomCheckoutActionUrl")]
        public string CustomCheckoutActionUrl { get; set; }
    }
}