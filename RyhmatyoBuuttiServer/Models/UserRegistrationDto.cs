using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords not matching.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public User toUser()
        {
            User user = new User();
            user.Email = this.Email;
            user.Username = this.Username;
            user.Password = this.Password;
            return user;
        }
    }
}
