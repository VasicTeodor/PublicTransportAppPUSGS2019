using System;

namespace PublicTransport.Api.Models
{
    public class Pricelist
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}