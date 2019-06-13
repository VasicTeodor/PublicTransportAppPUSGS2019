using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Dtos
{
    public class NewLineDto
    {
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public List<Station> Stations { get; set; }
        public List<Bus> Buses { get; set; }
    }
}
