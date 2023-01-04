using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using APS.DotNetSDK.Configuration;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests
{
    public class CheckStatusRequestCommand : RequestCommand
    {
        private string _returnThirdPartyResponseCodes;
        public CheckStatusRequestCommand()
        {
            AccessCode = SdkConfiguration.AccessCode;
            MerchantIdentifier = SdkConfiguration.MerchantIdentifier;
        }

        [JsonPropertyName("query_command")]
        public string Command => "CHECK_STATUS";

        [JsonPropertyName("fort_id")]
        public string FortId { get; set; }

        /// <summary>
        /// Should be "YES" or "NO"
        /// </summary>
        [JsonPropertyName("return_third_party_response_codes")]
        public string ReturnThirdPartyResponseCodes
        {
            get => _returnThirdPartyResponseCodes?.ToUpper();
            set => _returnThirdPartyResponseCodes = value;
        }
        public override void ValidateMandatoryProperties()
        {
            base.ValidateMandatoryProperties();

            if (string.IsNullOrEmpty(MerchantReference))
            {
                throw new ArgumentNullException($"MerchantReference", "MerchantReference is mandatory");
            }

            if (!string.IsNullOrEmpty(ReturnThirdPartyResponseCodes))
            {
                if (!(ReturnThirdPartyResponseCodes == "YES" || ReturnThirdPartyResponseCodes == "NO"))
                {
                    throw new ArgumentException("ReturnThirdPartyResponseCodes should be \"YES\" or \"NO\"", $"ReturnThirdPartyResponseCodes");
                }
            }
        }

        internal override string ToAnonymizedJson()
        {
            var anonymized = new CheckStatusRequestCommand()
            {
                AccessCode = this.AccessCode,
                MerchantIdentifier = this.MerchantIdentifier,
                Language = this.Language,
                Signature = this.Signature,

                MerchantReference = this.MerchantReference,
                FortId = this.FortId
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