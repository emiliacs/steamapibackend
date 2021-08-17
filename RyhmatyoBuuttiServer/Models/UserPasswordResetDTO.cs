using System.ComponentModel.DataAnnotations;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserPasswordResetDTO
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "New password is not matching to this confirmation password.")]
        public string ConfirmNewPassword { get; set; }
    }
}
