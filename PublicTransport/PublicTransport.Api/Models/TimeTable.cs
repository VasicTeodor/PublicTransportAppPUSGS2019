using System;
using System.Collections.Generic;

namespace PublicTransport.Api.Models
{
    public class TimeTable
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Day { get; set; }
        public Line Line { get; set; }
        public string Departures { get; set; }
        public int? LineId { get; set; }
    }
}