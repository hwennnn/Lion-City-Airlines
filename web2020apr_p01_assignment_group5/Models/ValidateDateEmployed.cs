using System;
using System.ComponentModel.DataAnnotations;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ValidateDateEmployed : ValidationAttribute
    {
        
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            string val = Convert.ToString(value);
            // Get the email value to validate 
            DateTime date = Convert.ToDateTime(value).Date;
            DateTime today = DateTime.Today;

            if (!val.Equals("") && (date < today || date > today.AddMonths(3)))
                // validation failed
                return new ValidationResult ("Please enter a valid date!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
