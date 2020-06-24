using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web2020apr_p01_assignment_group5.DAL;
using web2020apr_p01_assignment_group5.Models;
using Microsoft.AspNetCore.Identity;

namespace web2020apr_p01_assignment_group5.Controllers
{
    public class HomeController : Controller
    {
        private LoginDAL loginDAL = new LoginDAL();
        private AdminDAL adminDAL = new AdminDAL();
        private CustomerDAL customerContext = new CustomerDAL();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            string email = formData["txtLoginEmail"].ToString();
            string password = formData["txtPassword"].ToString();
            Console.WriteLine(email);
            Console.WriteLine(password);

            if (loginDAL.checkCustomer(email, password))
            {
                HttpContext.Session.SetString("Role", "Customer");

                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", email);
                // Finds Customer based on LoginID in Database
                Customer customer = customerContext.GetDetails(email);
                // Store Customer Name with the key "CustomerName"
                HttpContext.Session.SetString("CustomerName", customer.CustomerName);

                // redirect to customer homepage
                // store session data as customer
                return RedirectToAction("Index", "Customers");
            }

            else if (loginDAL.checkStaff(email, password))
            {
                Staff staff = adminDAL.GetSpecificStaffByEmail(email);

                HttpContext.Session.SetString("StaffName", staff.StaffName);
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", email);
                // Store user role “Staff” as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "Admin");
                // Redirect user to the "StaffMain" view through an action
                HttpContext.Session.SetString("LoginDT", DateTime.Now.ToString("dd-MMMM-y h:mm:ss tt"));

                return RedirectToAction("Index","Admin");
            }

            // Store an error message in TempData for display at the index view
            TempData["Message"] = "Invalid Login Credentials!";
            TempData["Action"] = "FAIL";
            // if wrong credetials found, attempt again
            return RedirectToAction("Login");
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }
    }
}