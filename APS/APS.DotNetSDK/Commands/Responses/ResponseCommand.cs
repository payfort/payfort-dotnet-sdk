using System;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses
{
    public abstract class ResponseCommand : Command
    {
        private string _language;

        [JsonPropertyName("access_code")]
        public string AccessCode { get; set; }

        [JsonPropertyName("merchant_identifier")]
        public string MerchantIdentifier { get; set; }

        [JsonPropertyName("language")]
        public string Language
        {
            get => _language?.ToLower();
            set => _language = value;
        }

        [JsonPropertyName("merchant_reference")]
        public string MerchantReference { get; set; }

        [JsonPropertyName("fort_id")]
        public string FortId { get; set; }

        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }

        [JsonPropertyName("response_code")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("reconciliation_reference")]
        public string ReconciliationReference { get; set; }
        
        [JsonPropertyName("processor_response_code")]
        public string ProcessorResponseCode { get; set; }
        
        public virtual void ValidateMandatoryProperties()
        {
            if (string.IsNullOrEmpty(Language))
            {
                throw new ArgumentNullException($"Language", "Language is mandatory");
            }
        }

        internal abstract string ToAnonymizedJson();
    }
}