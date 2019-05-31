using System.Collections.Generic;

namespace PublicTransport.Api.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Adress Adress { get; set; }
        public Location Location { get; set; }
        public ICollection<StationLine> StationLines { get; set; }
    }
}