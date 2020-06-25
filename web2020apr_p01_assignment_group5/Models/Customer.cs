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
    public class Customer
    {
        public int CustomerId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Your FullName!")]
        [StringLength(50,
            ErrorMessage = "Name cannot exceed 50 characters")]
        public string CustomerName { get; set; }

        [StringLength(50,
            ErrorMessage = "Nationality cannot exceed 50 characters")]
        public string? Nationality { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Contact Number")]
        [StringLength(50,
            ErrorMessage = "Contact Number cannot exceed 50 characters")]
        public string? TelNo { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please enter your Email Address!")]
        [StringLength(50,
            ErrorMessage = "Email Address cannot exceed 50 characters")]
        [EmailAddress]
        [ValidateEmailExists]
        public string EmailAddr { get; set; }

        public string Password { get; set; }
    }
}