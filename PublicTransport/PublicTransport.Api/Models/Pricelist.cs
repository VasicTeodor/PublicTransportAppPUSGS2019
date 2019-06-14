using System;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Models
{
    public class Pricelist
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Active { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}