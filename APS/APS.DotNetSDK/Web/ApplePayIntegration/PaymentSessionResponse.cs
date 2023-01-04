using System.Text.Json;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Web.ApplePayIntegration
{
    public class PaymentSessionResponse
    {
        [JsonPropertyName("epochTimestamp")]
        public long EpochTimestamp { get; set; }

        [JsonPropertyName("expiresAt")]
        public long ExpiresAt { get; set; }

        [JsonPropertyName("merchantSessionIdentifier")]
        public string MerchantSessionIdentifier { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("merchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonPropertyName("domainName")]
        public string DomainName { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("operationalAnalyticsIdentifier")]
        public string OperationalAnalyticsIdentifier { get; set; }

        [JsonPropertyName("retries")]
        public int Retries { get; set; }

        [JsonPropertyName("pspId")]
        public string PspId { get; set; }

        internal string ToAnonymizedJson()
        {
            var anonymized = new PaymentSessionResponse()
            {
                MerchantIdentifier = this.MerchantIdentifier,
                EpochTimestamp = this.EpochTimestamp,
                ExpiresAt = this.ExpiresAt,
                MerchantSessionIdentifier = this.MerchantSessionIdentifier,
                DomainName = this.DomainName,
                DisplayName = this.DisplayName,
                Signature = this.Signature,
                OperationalAnalyticsIdentifier = this.OperationalAnalyticsIdentifier,
                Retries = this.Retries,
                PspId = this.PspId,
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