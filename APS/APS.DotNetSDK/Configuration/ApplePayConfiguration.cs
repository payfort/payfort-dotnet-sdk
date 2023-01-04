using System;
using System.Text.Json.Serialization;
using System.Security.Cryptography.X509Certificates;
using APS.DotNetSDK.Signature;

namespace APS.DotNetSDK.Configuration
{
    public class ApplePayConfiguration
    {
        public ApplePayConfiguration(X509Certificate2 securityCertificate)
        {
            SecurityCertificate = securityCertificate;
        }

        [JsonPropertyName("AccessCode")]
        public string AccessCode { get; set; }

        [JsonPropertyName("MerchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonPropertyName("DisplayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("MerchantUid")]
        public string MerchantUid { get; set; }

        [JsonPropertyName("ResponseShaPhrase")]
        public string ResponseShaPhrase { get; set; }

        [JsonPropertyName("RequestShaPhrase")]
        public string RequestShaPhrase { get; set; }

        [JsonPropertyName("ShaType")]
        public string ShaTypeAsString { get; set; }
        
        [JsonPropertyName("DomainName")]
        public string DomainName { get; set; }

        [JsonIgnore]
        public ShaType ShaType { get; set; }

        public X509Certificate2 SecurityCertificate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(AccessCode))
            {
                throw new ArgumentNullException($"AccessCode", "AccessCode is needed for ApplePay configuration");
            }

            if (string.IsNullOrEmpty(ResponseShaPhrase))
            {
                throw new ArgumentNullException($"ResponseShaPhrase",
                    "ResponseShaPhrase is needed for ApplePay configuration");
            }

            if (string.IsNullOrEmpty(RequestShaPhrase))
            {
                throw new ArgumentNullException($"RequestShaPhrase",
                    "RequestShaPhrase is needed for ApplePay configuration");
            }

            if (string.IsNullOrEmpty(DisplayName))
            {
                throw new ArgumentNullException($"DisplayName", "DisplayName is needed for ApplePay configuration");
            }

            if (string.IsNullOrEmpty(MerchantUid))
            {
                throw new ArgumentNullException($"MerchantUid",
                    "MerchantUid is needed for ApplePay configuration");
            }

            if (string.IsNullOrEmpty(DomainName))
            {
                throw new ArgumentNullException($"DomainName",
                    "DomainName is needed for ApplePay configuration");
            }
        }
    }
}