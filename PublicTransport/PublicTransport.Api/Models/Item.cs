using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}