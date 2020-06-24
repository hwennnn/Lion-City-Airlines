using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using web2020apr_p01_assignment_group5.DAL;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private CustomerDAL customerContext = new CustomerDAL();
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate 
            string email = Convert.ToString(value);
            // Casting the validation context to the "Customer" model class 
            Customer customer = (Customer)validationContext.ObjectInstance;
            // Get the Customer Id from the Customer instance
            int customerId = customer.CustomerId;

            if (customerContext.IsEmailExist(email, customerId))
                // validation failed
                return new ValidationResult
                    ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
