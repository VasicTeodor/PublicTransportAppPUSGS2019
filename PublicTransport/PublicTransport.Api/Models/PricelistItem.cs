using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class PricelistItem
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public Pricelist Pricelist { get; set; }
        public decimal Price { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}