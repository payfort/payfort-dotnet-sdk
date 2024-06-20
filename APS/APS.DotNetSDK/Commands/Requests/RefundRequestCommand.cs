using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using APS.DotNetSDK.Commands.Requests.Airlines;

namespace APS.DotNetSDK.Commands.Requests
{
    public class RefundRequestCommand : RequestCommand
    {
        public RefundRequestCommand()
        {
        }

        [JsonPropertyName("command")]
        public string Command => "REFUND";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("maintenance_reference")]
        public string MaintenanceReference { get; set; }

        [JsonPropertyName("fort_id")]
        public string FortId { get; set; }

        [JsonPropertyName("order_description")]
        public string Description { get; set; }
        
        [JsonPropertyName("airline_data")]
        public AirlineData AirlineData { get; set; }

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
            var anonymized = new RefundRequestCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                MerchantReference = this.MerchantReference,
                Amount = this.Amount,
                Currency = this.Currency,
                MaintenanceReference = this.MaintenanceReference,
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