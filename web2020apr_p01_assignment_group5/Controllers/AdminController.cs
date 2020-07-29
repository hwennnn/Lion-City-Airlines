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

        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) || ((HttpContext.Session.GetString("Role") != "Admin") &&
                (HttpContext.Session.GetString("Role") != "Staff")))
            {
                return RedirectToAction("Index", "Home");
                // return to homepage if the user does not login as admin or staff
            }

            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                int staffID = (int)HttpContext.Session.GetInt32("StaffID");
                Staff staff = adminContext.GetSpecificStaffByID(staffID);
                PersonnelViewModel model = mapSpecificPersonneltoSchedule(staff);

                return View(model);
            }

            return View();
        }

        public ActionResult ViewPersonnels()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }
            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<PersonnelViewModel> personnelModel = mapPersonneltoSchedule();
            return View(personnelModel);
        }

        public ActionResult ViewFlightSchedules()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

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
            List<FlightRoute> routeList = adminContext.getAllFlightRoute();

            foreach (FlightRoute route in routeList)
            {
                ScheduleViewModel scheduleModel = new ScheduleViewModel();
                scheduleModel.RouteId = route.RouteId;
                scheduleModel.DepartureCity = route.DepartureCity;
                scheduleModel.DepartureCountry = route.DepartureCountry;
                scheduleModel.ArrivalCity = route.ArrivalCity;
                scheduleModel.ArrivalCountry = route.ArrivalCountry;
                scheduleModel.FlightDuration = route.FlightDuration;
                scheduleModel.scheduleList = adminContext.getSpecificScheduleList(route.RouteId);
                scheduleModelList.Add(scheduleModel);
            }
            return scheduleModelList;
        }

        public ActionResult CreatePersonnels()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }
            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();

            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersonnels(Staff staff)
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (staff == null)
            {
                return RedirectToAction("Index");
            }

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
                Value = "",
                Text = "Please select ..."
            });

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
                Value = "N", // i put in a random char because char connot be null technically
                Text = "Please select ..."
            });

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
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: FlightRoute/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightRoute(FlightRoute flightRoute)
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (flightRoute == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (adminContext.IsRouteExist(flightRoute))
                {
                    TempData["alert"] = "Flight Route with identical path already exists. Please enter new route...";
                    return View(flightRoute);
                }
                else
                {
                    //Add new FlightRoute record into database
                    adminContext.CreateFlightRoute(flightRoute);
                    //Return user to admin home page
                    return RedirectToAction("Index", "Admin");
                }
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
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

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
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (staff == null)
            {
                return RedirectToAction("Index");
            }

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
            flightStatusList.Add("Full");
            flightStatusList.Add("Delayed");
            flightStatusList.Add("Cancelled");
            return flightStatusList;
        }
        public ActionResult UpdateFlightScheduleStatus(int? id)
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

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
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (schedule == null)
            {
                return RedirectToAction("Index");
            }

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

        private List<Int32> RouteList()
        {
            List<Int32> routeIdList = new List<Int32>();
            List<FlightRoute> flightRouteList = new List<FlightRoute>();
            flightRouteList = adminContext.getAllFlightRoute();
            foreach(FlightRoute route in flightRouteList)
            {
                routeIdList.Add(route.RouteId);
            }
            return routeIdList;
        }

        public ActionResult CreateFlightSchedule()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index");
            }

            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["RouteIdList"] = RouteList();

            return View();
        }
    }
}