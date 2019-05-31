using Microsoft.AspNetCore.Identity;

namespace PublicTransport.Api.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}