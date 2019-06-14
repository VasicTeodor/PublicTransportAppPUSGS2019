using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}