using System;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests
{
    public abstract class RequestCommand : Command
    {
        private string _language;

        [JsonPropertyName("access_code")]
        public string AccessCode { get; protected set; }

        [JsonPropertyName("merchant_identifier")]
        public string MerchantIdentifier { get; protected set; }

        [JsonPropertyName("merchant_reference")]
        public string MerchantReference { get; set; }

        [JsonPropertyName("language")]
        public string Language
        {
            get => _language?.ToLower();
            set => _language = value;
        }

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