using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APS.DotNetSDK.Commands.Requests.Airlines
{
    public class Ticket
    {
        [JsonPropertyName("ticket_total_taxes")]
        public string TotalTaxes { get; set; }
        
        [JsonPropertyName("ticket_ticket_number")]
        public string TicketNumber { get; set; }
        
        [JsonPropertyName("ticket_conjunction_ticket_indicator")]
        public string ConjunctionTicketIndicator { get; set; }
        
        [JsonPropertyName("ticket_issue")]
        public TicketIssue Issue { get; set; }
        
        [JsonPropertyName("ticket_exchanged_ticket_number")]
        public string ExchangedTicketNumber { get; set; }
        
        [JsonPropertyName("ticket_tax_or_fee")]
        public List<TaxOrFee> TaxOrFee { get; set; }
         
        [JsonPropertyName("ticket_eTicket")]
        public string ETicket { get; set; }
         
        [JsonPropertyName("ticket_total_fare")]
        public string TotalFare { get; set; }
         
        [JsonPropertyName("ticket_restricted")]
        public string Restricted { get; set; }
         
        [JsonPropertyName("ticket_total_fees")]
        public string TotalFees { get; set; }
    }
}