using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class Itinerary
    {
        [JsonPropertyName("itinerary_number_in_party")] 
        public string NumberInParty { get; set; }
        
        [JsonPropertyName("itinerary_leg")] 
        public List<Leg> Legs { get; set; }
        
        [JsonPropertyName("itinerary_origin_country")]
        public string OriginCountry { get; set; }
    }
}