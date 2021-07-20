using System.ComponentModel.DataAnnotations;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserRegistrationDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords not matching.")]
        public string ConfirmPassword { get; set; }
    }
}
