using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web2020apr_p01_assignment_group5.DAL;
using web2020apr_p01_assignment_group5.Models;

namespace web2020apr_p01_assignment_group5.Controllers
{
    public class AdminController : Controller
    {

        private AdminDAL adminContext = new AdminDAL();

        public IActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||(HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
                // return to homepage if the user does not login as admin
            }

            return View();
        }

        public ActionResult ViewPersonnels()
        {
            List<Staff> staffList = adminContext.getAllStaff();
            return View(staffList);
        }
    }
}