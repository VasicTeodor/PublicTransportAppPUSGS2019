using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PublicTransport.Api.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string UserType { get; set; }
        public string AccountStatus { get; set; }
        public string DocumentUrl { get; set; }
        public string PublicId { get; set; } // for photo
        public bool Verified { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}