using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web2020apr_p01_assignment_group5.DAL;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ChangePassword
    {
        [Display(Name = "CurrentPassword")]
        public string Password { get; set; }

        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
