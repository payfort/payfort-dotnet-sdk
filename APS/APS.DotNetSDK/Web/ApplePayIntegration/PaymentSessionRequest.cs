using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public class PaymentSessionRequest
    {
        [JsonPropertyName("merchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("initiative")]
        public string Initiative { get; set; }

        [JsonPropertyName("initiativeContext")]
        public string InitiativeContext { get; set; }

        public void ValidateParameters()
        {
            if (string.IsNullOrEmpty(MerchantIdentifier))
            {
                throw new ArgumentNullException(MerchantIdentifier, "MerchantIdentifier is mandatory");
            }

            if (string.IsNullOrEmpty(DisplayName))
            {
                throw new ArgumentNullException(DisplayName, "DisplayName is mandatory");
            }

            if (string.IsNullOrEmpty(Initiative))
            {
                throw new ArgumentNullException(Initiative, "Initiative is mandatory");
            }

            if (string.IsNullOrEmpty(InitiativeContext))
            {
                throw new ArgumentNullException(InitiativeContext, "InitiativeContext is mandatory");
            }
        }

        internal string ToAnonymizedJson()
        {
            var anonymized = new PaymentSessionRequest()
            {
                MerchantIdentifier = this.MerchantIdentifier,
                DisplayName = this.DisplayName,
                Initiative = this.Initiative,
                InitiativeContext = this.InitiativeContext,
            };

            var serialized = JsonSerializer.Serialize(anonymized,
                   new JsonSerializerOptions
                   {
                       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                   });

            return serialized;
        }
    }
}