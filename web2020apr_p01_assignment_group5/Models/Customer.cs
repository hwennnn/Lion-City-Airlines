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

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string? TelNo { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address!")]
        [StringLength(50)]
        [EmailAddress]
        [ValidateEmailExists]
        public string EmailAddr { get; set; }

        public string Password { get; set; }
    }
}