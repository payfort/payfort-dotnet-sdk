using APS.DotNetSDK.Commands;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Commands
{
    public class TestRequestCommand : Command
    {
        private string? _language;

        public TestRequestCommand()
        {
        }

        [JsonPropertyName("access_code")]
        public string? AccessCode { get; set; }

        [JsonPropertyName("merchant_identifier")]
        public string? MerchantIdentifier { get; set; }

        [JsonPropertyName("language")]
        public string? Language
        {
            get => _language?.ToLower();
            set => _language = value;
        }

        public virtual void ValidateMandatoryProperties()
        {
            if (string.IsNullOrEmpty(Language))
            {
                throw new ArgumentNullException($"Language", "Language is mandatory");
            }
        }
    }
}
