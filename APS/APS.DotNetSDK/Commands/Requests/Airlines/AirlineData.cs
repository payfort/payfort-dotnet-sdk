using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class AirlineData
    {
        [JsonPropertyName("airline_plan_number")]
        public double PlanNumber { get; set; }

        [JsonPropertyName("airline_transaction_type")]
        public string TransactionType { get; set; }

        [JsonPropertyName("airline_booking_reference")]
        public string BookingReference { get; set; }

        [JsonPropertyName("airline_document_type")]
        public string DocumentType { get; set; }

        [JsonPropertyName("airline_passengers_list")]
        public List<Passenger> Passengers { get; set; }

        [JsonPropertyName("airline_ticket")] 
        public Ticket Ticket { get; set; }

        [JsonPropertyName("airline_itinerary")]
        public Itinerary Itinerary { get; set; }
    }
}