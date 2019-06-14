using System;

namespace PublicTransport.Api.Dtos
{
    public class NewPricelistDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}