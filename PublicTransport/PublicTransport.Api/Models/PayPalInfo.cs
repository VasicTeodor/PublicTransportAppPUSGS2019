using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Models
{
    public class PayPalInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string PaymentMethod { get; set; }
        public string PayerAccountStatus { get; set; }
        public string Status { get; set; }
        public string Time { get; set; }
        public string Currency { get; set; }
        public string Total { get; set; }
        [Timestamp]
        public byte[] TableVersion { get; set; }
    }
}
