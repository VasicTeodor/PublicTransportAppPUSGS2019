using System;

namespace PublicTransport.Api.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string TicketType { get; set; }
        public bool IsValid { get; set; }
        public User User { get; set; }
        public PricelistItem PriceInfo { get; set; }
    }
}