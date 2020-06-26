using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web2020apr_p01_assignment_group5.DAL;
using web2020apr_p01_assignment_group5.Models;

namespace web2020apr_p01_assignment_group5.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerDAL customerContext = new CustomerDAL();

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Customer/CreateCustomerProfile
        public ActionResult CreateCustomerProfile()
        {
            return View();
        }

        // POST: Customer/CreateCustomerProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomerProfile(Customer customer)
        {
            //in case of the need to return to CreateCustomerProfile.cshtml view
            if (ModelState.IsValid)
            {
                //Add customer record to database
                customer.CustomerId = customerContext.Addcustomer(customer);
                //Redirect user to Home/Login view
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Input validation fails, return to the CreateCustomerProfile.cshtml view
                //to display error message
                return View(customer);
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                Customer customer = customerContext.GetDetails(HttpContext.Session.GetString("LoginID"));
                if (customer.Password == changePassword.CurrentPassword && changePassword.NewPassword == changePassword.ConfirmPassword)
                {
                    customerContext.ChangePassword(customer, changePassword);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct.");
                }
            }
            return View();
        }

    }
}