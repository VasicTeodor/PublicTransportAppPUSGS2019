using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public Location Location { get; set; }
        public ICollection<StationLine> StationLines { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}