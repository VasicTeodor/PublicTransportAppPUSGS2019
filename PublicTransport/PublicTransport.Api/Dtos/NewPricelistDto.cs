using System;

namespace PublicTransport.Api.Dtos
{
    public class NewPricelistDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Active { get; set; }
        public decimal PriceHourly { get; set; }
        public decimal PriceDaily { get; set; }
        public decimal PriceMonthly { get; set; }
        public decimal PriceAnnual { get; set; }
        public string Type { get; set; }
        public int IdHourly { get; set; }
        public int IdDaily { get; set; }
        public int IdMonthly { get; set; }
        public int IdAnnual { get; set; }
    }
}