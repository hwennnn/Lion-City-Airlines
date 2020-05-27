using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web2020apr_p01_assignment_group5.DAL;
using Microsoft.AspNetCore.Mvc;

namespace web2020apr_p01_assignment_group5.Controllers
{
    public class AdminController : Controller
    {

        private AdminDAL adminContext = new AdminDAL();

        public IActionResult Index()
        {
            adminContext.getAllStaff();
            return View();
        }
    }
}