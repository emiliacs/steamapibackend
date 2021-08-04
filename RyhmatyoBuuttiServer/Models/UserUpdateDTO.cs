using System.ComponentModel.DataAnnotations;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserUpdateDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Username { get; set; }
    }
}
