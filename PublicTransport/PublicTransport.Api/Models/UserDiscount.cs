using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class UserDiscount
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}