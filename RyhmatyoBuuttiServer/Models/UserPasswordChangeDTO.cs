using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserPasswordChangeDTO
    {
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = "New password is required.")]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string NewPassword { get; set; }
        
        [Compare("NewPassword", ErrorMessage = "New password is not matching to this confirmation password.")]
        public string ConfirmNewPassword { get; set; }
    }
}
