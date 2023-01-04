using APS.DotNetSDK.Commands;
using APS.DotNetSDK.Tests.Commands;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Tests.Signature.Models
{
    internal class ThreeLevelsPropertyRequestCommand : TestRequestCommand
    {
        [JsonPropertyName("customer_detail")]
        public Customer? CustomerDetail { get; set; }
    }

    internal class Customer
    {
        [JsonPropertyName("customer_email")]
        public string? CustomerEmail { get; set; }

        [JsonPropertyName("customer_billing")]
        public CustomerBillingDetails? BillingDetails { get; set; }
    }

    internal class CustomerBillingDetails
    {
        [JsonPropertyName("billing_streetname")]
        public string? StreetName { get; set; }
        [JsonPropertyName("billing_country")]
        public BillingCountry? BillingCountry { get; set; }
    }

    internal class BillingCountry
    {
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        [JsonPropertyName("countrycode")]
        public int CountryCode { get; set; }
    }
}
