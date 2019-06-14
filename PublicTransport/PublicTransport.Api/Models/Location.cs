using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Location
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}