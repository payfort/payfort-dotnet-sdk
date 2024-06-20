using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class Passenger
    {
        [JsonPropertyName("passenger_first_name")]
        public string FirstName { get; set; }
        
        [JsonPropertyName("passenger_frequent_flyer_number")]
        public string FrequentFlyerNumber { get; set; }
       
        [JsonPropertyName("passenger_last_name")]
        public string LastName { get; set; }
        
        [JsonPropertyName("passenger_middle_name")]
        public string MiddleName { get; set; }
         
        [JsonPropertyName("passenger_specific_information")]
        public string SpecificInformation { get; set; }
        
        [JsonPropertyName("passenger_title")]
        public string Title { get; set; }
    }
}