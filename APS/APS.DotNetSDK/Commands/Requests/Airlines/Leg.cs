using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class Leg
    {
         
        [JsonPropertyName("leg_carrier_code")]
        public string CarrierCode { get; set; }
         
        [JsonPropertyName("leg_conjunction_ticket_number")]
        public string ConjunctionTicketNumber { get; set; }
         
        [JsonPropertyName("leg_coupon_number")]
        public string CouponNumber { get; set; }
         
        [JsonPropertyName("leg_departure_airport")]
        public string DepartureAirport { get; set; }
         
        [JsonPropertyName("leg_departure_date")]
        public string DepartureDate { get; set; }
         
        [JsonPropertyName("leg_departure_tax")]
        public string DepartureTax { get; set; }
         
        [JsonPropertyName("leg_departure_time")]
        public string DepartureTime { get; set; }
         
        [JsonPropertyName("leg_destination_airport")]
        public string DestinationAirport { get; set; }
         
        [JsonPropertyName("leg_destination_arrival_date")]
        public string DestinationArrivalDate { get; set; }
         
        [JsonPropertyName("leg_destination_arrival_time")]
        public string DestinationArrivalTime { get; set; }
         
        [JsonPropertyName("leg_endorsements_restrictions")]
        public string EndorsementsRestrictions { get; set; }
         
        [JsonPropertyName("leg_exchange_ticket_number")]
        public string ExchangeTicketNumber { get; set; }
         
        [JsonPropertyName("leg_fare")]
        public string Fare { get; set; }
         
        [JsonPropertyName("leg_fare_basis")]
        public string FareBasis { get; set; }
         
        [JsonPropertyName("leg_fees")]
        public string Fees { get; set; }
         
        [JsonPropertyName("leg_flight_number")]
        public string FlightNumber { get; set; }
         
        [JsonPropertyName("leg_stopover_permitted")]
        public string StopoverPermitted { get; set; }
         
        [JsonPropertyName("leg_taxes")]
        public string Taxes { get; set; }
         
        [JsonPropertyName("leg_travel_class")]
        public string TravelClass { get; set; }
    }
}