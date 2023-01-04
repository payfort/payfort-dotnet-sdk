﻿using System.Text.Json;
using APS.DotNetSDK.Utils;
using APS.DotNetSDK.Exceptions;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Responses
{
    public class  RefundResponseCommand : ResponseCommandWithNotification
    {
        private const string CommandKey = "command";

        [JsonPropertyName("command")]
        public override string Command => "REFUND";

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("maintenance_reference")]
        public string MaintenanceReference { get; set; }

        [JsonPropertyName("order_description")]
        public string Description { get; set; }

        public override void BuildNotificationCommand(IDictionary<string, string> dictionaryObject)
        {
            if (!dictionaryObject.Keys.Contains(CommandKey))
            {
                throw new InvalidNotification(
                    "Response does not contain any command. Please contact the SDK provider.");
            }

            var responseCommand = dictionaryObject.CreateObject<RefundResponseCommand>();

            if (responseCommand == null)
            {
                throw new InvalidNotification(
                    $"There was an issue when mapping notification request: [" +
                    $"{dictionaryObject.JoinElements(",")}] to RefundResponseCommand");
            }

            if (dictionaryObject["command"] != Command)
            {
                throw new InvalidNotification(
                    $"Invalid Command received from payment gateway:{dictionaryObject["command"]}");
            }

            Signature = responseCommand.Signature;

            AccessCode = responseCommand.AccessCode;
            MerchantIdentifier = responseCommand.MerchantIdentifier;
            Language = responseCommand.Language;
            MerchantReference = responseCommand.MerchantReference;
            FortId = responseCommand.FortId;
            ResponseMessage = responseCommand.ResponseMessage;
            ResponseCode = responseCommand.ResponseCode;
            Status = responseCommand.Status;
            ReconciliationReference = responseCommand.ReconciliationReference;
            ProcessorResponseCode = responseCommand.ProcessorResponseCode;

            Amount = responseCommand.Amount;
            Currency = responseCommand.Currency;
            MaintenanceReference = responseCommand.MaintenanceReference;
            Description = responseCommand.Description;
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new RefundResponseCommand()
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

                Amount = this.Amount,
                Currency = this.Currency,
                MaintenanceReference = this.MaintenanceReference,
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