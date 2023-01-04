using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses
{
    public class CheckStatusResponseCommand : ResponseCommand
    {
        [JsonPropertyName("query_command")]
        public string Command => "CHECK_STATUS";

        [JsonPropertyName("transaction_status")]
        public string TransactionStatus { get; set; }

        [JsonPropertyName("transaction_code")]
        public string TransactionCode { get; set; }

        [JsonPropertyName("transaction_message")]
        public string TransactionMessage { get; set; }

        [JsonPropertyName("refunded_amount")]
        public double? RefundedAmount { get; set; }

        [JsonPropertyName("captured_amount")]
        public double? CapturedAmount { get; set; }

        [JsonPropertyName("authorized_amount")]
        public double? AuthorizedAmount { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new CheckStatusResponseCommand()
            {
                Signature = this.Signature,

                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                MerchantReference = this.MerchantReference,
                FortId = this.FortId,
                ResponseCode = this.ResponseCode,
                ResponseMessage = this.ResponseMessage,
                Status = this.Status,
                ProcessorResponseCode = this.ProcessorResponseCode,
                ReconciliationReference = this.ReconciliationReference,

                TransactionStatus = this.TransactionStatus,
                TransactionCode = this.TransactionCode,
                TransactionMessage = this.TransactionMessage,
                RefundedAmount = this.RefundedAmount,
                CapturedAmount = this.CapturedAmount,
                AuthorizedAmount = this.AuthorizedAmount,
                AuthorizationCode = this.AuthorizationCode
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