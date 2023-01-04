using System.Text.Json;
using APS.DotNetSDK.Utils;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using APS.DotNetSDK.Commands.Responses.Installments;

namespace APS.DotNetSDK.Commands.Responses
{
    public class GetInstallmentsResponseCommand : ResponseCommand
    {
        [JsonPropertyName("query_command")]
        public string Command => "GET_INSTALLMENTS_PLANS";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("issuer_code")]
        public string IssuerCode { get; set; }

        [IgnoreOnSignatureCalculation(true)]
        [JsonPropertyName("installment_detail")]
        public InstallmentDetails InstallmentDetail { get; set; }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new GetInstallmentsResponseCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                ResponseCode = this.ResponseCode,
                ResponseMessage = this.ResponseMessage,
                Status = this.Status,
                ProcessorResponseCode = this.ProcessorResponseCode,
                ReconciliationReference = this.ReconciliationReference,

                Amount = this.Amount,
                Currency = this.Currency,
                IssuerCode = this.IssuerCode,
                InstallmentDetail = this.InstallmentDetail
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
