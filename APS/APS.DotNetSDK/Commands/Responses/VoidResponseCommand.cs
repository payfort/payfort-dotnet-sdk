using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Exceptions;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses
{
    public class  VoidResponseCommand : ResponseCommandWithNotification
    {
        private const string CommandKey = "command";

        [JsonPropertyName("command")]
        public override string Command => "VOID_AUTHORIZATION";

        [JsonPropertyName("order_description")]
        public string Description { get; set; }

        public override void BuildNotificationCommand(IDictionary<string, string> dictionaryObject)
        {
            if (!dictionaryObject.Keys.Contains(CommandKey))
            {
                throw new InvalidNotification(
                    "Response does not contain any command. Please contact the SDK provider.");
            }

            var responseCommand = dictionaryObject.CreateObject<VoidResponseCommand>();

            if (responseCommand == null)
            {
                throw new InvalidNotification(
                    $"There was an issue when mapping notification request: [" +
                    $"{dictionaryObject.JoinElements(",")}] to VoidResponseCommand");
            }

            if (dictionaryObject["command"] != Command)
            {
                throw new InvalidNotification(
                    $"Invalid Command received from payment gateway:{dictionaryObject["command"]}");
            }

            AccessCode = responseCommand.AccessCode;
            MerchantIdentifier = responseCommand.MerchantIdentifier;
            Language = responseCommand.Language;
            Signature = responseCommand.Signature;

            MerchantReference = responseCommand.MerchantReference;
            FortId = responseCommand.FortId;
            ResponseMessage = responseCommand.ResponseMessage;
            ResponseCode = responseCommand.ResponseCode;
            Status = responseCommand.Status; 
            ReconciliationReference = responseCommand.ReconciliationReference;
            ProcessorResponseCode = responseCommand.ProcessorResponseCode;

            Description = responseCommand.Description;
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new VoidResponseCommand()
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

                MerchantReference = this.MerchantReference,
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