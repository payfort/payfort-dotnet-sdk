using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using APS.DotNetSDK.Configuration;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests
{
    public class CaptureRequestCommand : RequestCommand
    {
        public CaptureRequestCommand()
        {
            AccessCode = SdkConfiguration.AccessCode;
            MerchantIdentifier = SdkConfiguration.MerchantIdentifier;
        }

        [JsonPropertyName("command")]
        public string Command => "CAPTURE";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("fort_id")]
        public string FortId { get; set; }

        [JsonPropertyName("order_description")]
        public string Description { get; set; }

        public override void ValidateMandatoryProperties()
        {
            base.ValidateMandatoryProperties();

            if (string.IsNullOrEmpty(MerchantReference))
            {
                throw new ArgumentNullException($"MerchantReference", "MerchantReference is mandatory");
            }

            if (Amount <= 0)
            {
                throw new ArgumentNullException($"Amount", "Amount is mandatory");
            }

            if (string.IsNullOrEmpty(Currency))
            {
                throw new ArgumentNullException($"Currency", "Currency is mandatory");
            }
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new CaptureRequestCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                MerchantReference = this.MerchantReference,
                Amount = this.Amount,
                Currency = this.Currency,
                FortId = this.FortId,
                Description = this.Description
            };

            var serialized = JsonSerializer.Serialize(anonymized,
                   new JsonSerializerOptions
                   {
                       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                   });

            return serialized;
        }
    }
}