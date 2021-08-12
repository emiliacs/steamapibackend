using System.ComponentModel.DataAnnotations;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserForgottenPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
