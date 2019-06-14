using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Line
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public ICollection<StationLine> Stations { get; set; }
        public ICollection<Bus> Buses { get; set; }
        public int? TimetableId { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
        //public TimeTable Timetable { get; set; }
    }
}