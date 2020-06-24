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

        public ActionResult ViewFlightSchedules()
        {
            List<ScheduleViewModel> flightSchedule = mapScheduletoRoute();
            return View(flightSchedule);
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
                             RouteId = schedule.RouteId,
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

        public List<ScheduleViewModel> mapScheduletoRoute()
        {
            List<ScheduleViewModel> scheduleModelList = new List<ScheduleViewModel>();
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            scheduleList = adminContext.getAllFlightSchedule();

            foreach (FlightSchedule schedule in scheduleList)
            {
                ScheduleViewModel scheduleModel = new ScheduleViewModel();
                scheduleModel.ScheduleId = schedule.ScheduleId;
                scheduleModel.FlightNumber = schedule.FlightNumber;
                scheduleModel.RouteId = schedule.RouteId;
                scheduleModel.AircraftId = schedule.AircraftId;
                scheduleModel.DepartureDateTime = schedule.DepartureDateTime;
                scheduleModel.ArrivalDateTime = schedule.ArrivalDateTime;
                scheduleModel.EconomyClassPrice = schedule.EconomyClassPrice;
                scheduleModel.BusinessClassPrice = schedule.BusinessClassPrice;
                scheduleModel.Status = schedule.Status;
                scheduleModel.Route = adminContext.getSpecificRoute(schedule.RouteId);
                scheduleModelList.Add(scheduleModel);
            }

            return scheduleModelList;
        }

        public ActionResult CreatePersonnel()
        {
            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();

            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersonnel(Staff staff)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();
            if (ModelState.IsValid)
            {
                //Add staff record to database
                staff.StaffId = adminContext.CreatePersonnel(staff);
                //Redirect user to Staff/Index view
                return RedirectToAction("Index","Admin");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(staff);
            }
        }

        private List<SelectListItem> GetVocation()
        {
            List<SelectListItem> vocations = new List<SelectListItem>();

            vocations.Add(new SelectListItem
            {
                Value = "Pilot",
                Text = "Pilot"
            });

            vocations.Add(new SelectListItem
            {
                Value = "Flight Attendant",
                Text = "Flight Attendant"
            });
 
            return vocations;
        }

        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> vocations = new List<SelectListItem>();

            vocations.Add(new SelectListItem
            {
                Value = "M",
                Text = "Male"
            });

            vocations.Add(new SelectListItem
            {
                Value = "F",
                Text = "Female"
            });

            return vocations;
        }

        public ActionResult CreateFlightRoute()
        {
            return View();
        }
        // POST: FlightRoute/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightRoute(FlightRoute flightRoute)
        {
            if (ModelState.IsValid)
            {
                //Add new FlightRoute record into database
                flightRoute.RouteId = adminContext.CreateFlightRoute(flightRoute);
                //Return user to admin home page
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(flightRoute);
            }
        }
    }
}