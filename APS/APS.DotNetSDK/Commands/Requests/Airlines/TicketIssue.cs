using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class TicketIssue
    {
        
        [JsonPropertyName("issue_address")]
        public string Address { get; set; }
         
        [JsonPropertyName("issue_city")]
        public string City { get; set; }
         
        [JsonPropertyName("issue_date")]
        public string Date { get; set; }
         
        [JsonPropertyName("issue_travel_agent_name")]
        public string TravelAgentName { get; set; }
         
        [JsonPropertyName("issue_carrier_code")]
        public string CarrierCode { get; set; }
         
        [JsonPropertyName("issue_carrier_name")]
        public string CarrierName { get; set; }
         
        [JsonPropertyName("issue_travel_agent_code")]
        public string TravelAgentCode { get; set; }
         
        [JsonPropertyName("issue_country")]
        public string Country { get; set; }
    }
}