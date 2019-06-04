using System.Collections.Generic;

namespace PublicTransport.Api.Models
{
    public class Line
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public ICollection<StationLine> Stations { get; set; }
        public ICollection<Bus> Buses { get; set; }
    }
}