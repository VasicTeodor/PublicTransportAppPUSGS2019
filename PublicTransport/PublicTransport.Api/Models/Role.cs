using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PublicTransport.Api.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}