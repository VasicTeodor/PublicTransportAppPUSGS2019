using System;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Api.Dtos
{
    public class UserForUpdateDto
    {
        [Required]
        public string UserType { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "You must specify password at least 4 characters long")]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "You must specify password at least 4 characters long")]
        public string OldPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}