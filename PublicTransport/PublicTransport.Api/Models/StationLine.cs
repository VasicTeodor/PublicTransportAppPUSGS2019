﻿namespace PublicTransport.Api.Models
{
    public class StationLine
    {
        public int LineId { get; set; }
        public int StationId { get; set; }
        public Line Line { get; set; }
        public Station Station { get; set; }
    }
}