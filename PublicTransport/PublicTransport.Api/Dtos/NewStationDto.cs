using System.Collections.Generic;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Dtos
{
    public class NewStationDto
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public List<Line> Lines { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}