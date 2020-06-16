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
            List<PersonnelViewModel> personnelModel = mapPersonneltoSchedule();
            return View(personnelModel);
        }

        public List<PersonnelViewModel> mapPersonneltoSchedule()
        {
            List<PersonnelViewModel> personnelsModelList = new List<PersonnelViewModel>();
            List<Staff> staffList = adminContext.getAllStaff();

            foreach (Staff staff in staffList)
            {
                PersonnelViewModel personnelsModel = new PersonnelViewModel();
                personnelsModel.StaffId = staff.StaffId;
                personnelsModel.StaffName = staff.StaffName;
                personnelsModel.Vocation = staff.Vocation;

                List<FlightCrew> flightCrewList = adminContext.getSpecificFlightCrew(staff.StaffId);
                List<FlightSchedule> scheduleList = new List<FlightSchedule>();
                foreach (FlightCrew crew in flightCrewList)
                {
                    FlightSchedule schedule = adminContext.getSpecificSchedule(crew.ScheduleID);
                    scheduleList.Add(
                         new FlightSchedule
                         {
                             ScheduleId = schedule.ScheduleId,
                             FlightNumber = schedule.FlightNumber,
                             RouteID = schedule.RouteID,
                             DepartureDateTime = schedule.DepartureDateTime,
                             ArrivalDateTime = schedule.ArrivalDateTime,
                             Status = schedule.Status,
                             Role = crew.Role

                         }) ;
                }
                personnelsModel.flightScheduleList = scheduleList;

                personnelsModelList.Add(personnelsModel);

            }

            return personnelsModelList;
        }
    }
}