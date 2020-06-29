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
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
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
                personnelsModel.Status = staff.Status;

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

                         });
                }
                personnelsModel.flightScheduleList = scheduleList;

                personnelsModelList.Add(personnelsModel);

            }

            return personnelsModelList;
        }

        public PersonnelViewModel mapSpecificPersonneltoSchedule(Staff staff)
        {
            PersonnelViewModel personnelsModel = new PersonnelViewModel();
            personnelsModel.StaffId = staff.StaffId;
            personnelsModel.StaffName = staff.StaffName;
            personnelsModel.Vocation = staff.Vocation;
            personnelsModel.Status = staff.Status;

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

                     });
            }
            personnelsModel.flightScheduleList = scheduleList;

            return personnelsModel;
        }

        public List<ScheduleViewModel> mapScheduletoRoute()
        {
            List<ScheduleViewModel> scheduleModelList = new List<ScheduleViewModel>();
            List<FlightSchedule> scheduleList = adminContext.getAllFlightSchedule();

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

        public ActionResult CreatePersonnels()
        {
            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();

            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersonnels(Staff staff)
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
                return RedirectToAction("Index", "Admin");
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
            List<SelectListItem> genders = new List<SelectListItem>();

            genders.Add(new SelectListItem
            {
                Value = "M",
                Text = "Male"
            });

            genders.Add(new SelectListItem
            {
                Value = "F",
                Text = "Female"
            });

            return genders;
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

        // GET: Staff/Edit/5
        public ActionResult UpdatePersonnelStatus(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            //if ((HttpContext.Session.GetString("Role") == null) ||
            //(HttpContext.Session.GetString("Role") != "Admin"))
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            Staff staff = adminContext.GetSpecificStaffByID(id.Value);

            if (staff == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePersonnelStatus(Staff staff)
        {
            bool isUpdateValid = true;

            PersonnelViewModel personnelModel = mapSpecificPersonneltoSchedule(staff);
            foreach (FlightSchedule schedule in personnelModel.flightScheduleList)
            {
                if (schedule.DepartureDateTime.Date >= DateTime.Today)
                {
                    isUpdateValid = false;
                }
            }

            if (isUpdateValid)
            {
                Console.WriteLine("valid update");
                //Update staff status to database
                adminContext.updatePersonnelStatus(staff);
                TempData["alert"] = "The staff's status has been updated.";
                return RedirectToAction("ViewPersonnels");
            }
            else
            {
                Console.WriteLine("invalid update");
                TempData["alert"] = "The update action is fail as the staff has already been assigned upcoming schedules.";
                return View(staff);
            }
        }

        private List<String> FlightStatusList()
        {
            List<String> flightStatusList = new List<String>();
            flightStatusList.Add("Opened");
            flightStatusList.Add("Full");
            flightStatusList.Add("Delayed");
            flightStatusList.Add("Cancelled");
            return flightStatusList;
        }
        public ActionResult UpdateFlightScheduleStatus(int? id)
        {
            //Check if ID exists in database
            if (id == null)
            {
                //Return user to index
                return RedirectToAction("Index");
            }
            //Declaring attribute for calling status list in view
            ViewData["StatusList"] = FlightStatusList();
            //Declaring selected FlightSchedule
            FlightSchedule schedule = adminContext.getSpecificSchedule(id.Value);

            //Check if schedule object exists
            if (schedule == null)
            {
                //Return user to index
                return RedirectToAction("Index");
            }
            //Display view with schedule as item
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFlightScheduleStatus(FlightSchedule schedule)
        {
            if (ModelState.IsValid)
            {
                adminContext.updateFlightScheduleStatus(schedule, schedule.Status);
                TempData["alert"] = "Flight Schedule Status has been updated.";
                return RedirectToAction("ViewFlightSchedules");
            }
            else
            {
                TempData["alert"] = "An error occurred. Sending User back to List...";
                return View(schedule);
            }
        }
    }
}