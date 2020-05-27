using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web2020apr_p01_assignment_group5.Models;

namespace web2020apr_p01_assignment_group5.Controllers
{
    public class HomeController : Controller
    {
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
            string loginID = formData["txtLoginEmail"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();
            Console.WriteLine(loginID);
            Console.WriteLine(password);

            if (loginID == "Peter_Ghim@gmail.com" && password == "p@55PG")
            {
                // to be added to check with the customer database for login credentials
                // redirect to customer homepage
                // store session data as customer
                return RedirectToAction("Index");
            }

            else if (loginID == "s1234567@lca.com" && password == "p@55Staff")
            {
                // redirect to admin homepage
                // store session data as admin
                return RedirectToAction("Index","Admin");
            }

            // Store an error message in TempData for display at the index view
            TempData["Message"] = "Invalid Login Credentials!";
            // if wrong credetials found, attempt again
            return RedirectToAction("Login");
        }
    }
}
