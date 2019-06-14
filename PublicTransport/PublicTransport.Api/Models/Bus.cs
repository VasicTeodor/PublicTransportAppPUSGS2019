using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public int BusNumber { get; set; }
        public bool InUse { get; set; }
        public Line Line { get; set; }
        public int? LineId { get; set; }

        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}