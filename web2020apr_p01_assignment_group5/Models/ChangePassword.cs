using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage ="Current password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword",ErrorMessage =
            "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
