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

            List<RouteScheduleViewModel> flightSchedule = mapScheduletoRoute();
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
                             AircraftId = schedule.AircraftId,
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
                         AircraftId = schedule.AircraftId,
                         DepartureDateTime = schedule.DepartureDateTime,
                         ArrivalDateTime = schedule.ArrivalDateTime,
                         Status = schedule.Status,
                         Role = crew.Role

                     });
            }
            personnelsModel.flightScheduleList = scheduleList;

            return personnelsModel;
        }

        public List<RouteScheduleViewModel> mapScheduletoRoute()
        {
            List<RouteScheduleViewModel> scheduleModelList = new List<RouteScheduleViewModel>();
            List<FlightRoute> routeList = adminContext.getAllFlightRoute();

            foreach (FlightRoute route in routeList)
            {
                RouteScheduleViewModel scheduleModel = new RouteScheduleViewModel();
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
                if (Convert.ToDateTime(schedule.DepartureDateTime).Date >= DateTime.Today)
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

        private List<String> RouteList()
        {
            List<String> routeIdList = new List<String>();
            List<FlightRoute> flightRouteList = new List<FlightRoute>();
            flightRouteList = adminContext.getAllFlightRoute();
            foreach(FlightRoute route in flightRouteList)
            {
                routeIdList.Add(Convert.ToString(route.RouteId));
            }
            return routeIdList;
        }

        private List<String> AircraftList()
        {
            List<String> aircraftIdList = new List<String>();
            aircraftIdList.Add("Select an Aircraft ID");
            List<Aircraft> aircraftList = new List<Aircraft>();
            aircraftList = adminContext.getAllAircraft();
            foreach(Aircraft aircraft in aircraftList)
            {
                aircraftIdList.Add(Convert.ToString(aircraft.AircraftId));
            }
            return aircraftIdList;
        }

        public ActionResult CreateFlightSchedule(int? id)
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
            {
                ViewData["RouteIdList"] = RouteList();

                ViewData["AircraftIdList"] = AircraftList();

                ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
                return View(scheduleViewModel);
            }
            else
            {
                ViewData["AircraftIdList"] = AircraftList();

                ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
                int rid = id.Value;
                scheduleViewModel.RouteId = rid;
                scheduleViewModel.Route = adminContext.getSpecificRoute(rid);
                if (adminContext.IsFlightDurationNull(rid))
                {
                    scheduleViewModel.DepartureDateTime = DateTime.Today;
                    return View(scheduleViewModel);
                }
                else
                {
                    return View(scheduleViewModel);
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightSchedule(ScheduleViewModel scheduleViewModel)
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

            if (scheduleViewModel == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (scheduleViewModel.AircraftId == "Select an Aircraft ID")
                {
                    scheduleViewModel.AircraftId = null;
                }
                if (scheduleViewModel.DepartureDateTime == DateTime.Today)
                {
                    scheduleViewModel.DepartureDateTime = null;
                }
                FlightSchedule flightSchedule = mapViewModeltoSchedule(scheduleViewModel);
                if (adminContext.IsFlightDurationNull(flightSchedule.RouteId))
                {
                    flightSchedule.DepartureDateTime = null;
                    TempData["Alert"] = "FlightSchedule has been created with no DepartureDateTime as FlightDuration is NULL";
                    adminContext.CreateFlightSchedule(flightSchedule);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    FlightRoute route = adminContext.getSpecificRoute(flightSchedule.RouteId);
                    if (flightSchedule.DepartureDateTime.HasValue)
                    {
                        flightSchedule.ArrivalDateTime = Convert.ToDateTime(flightSchedule.DepartureDateTime).AddHours(Convert.ToDouble(route.FlightDuration));
                    }
                    else
                    {
                        flightSchedule.ArrivalDateTime = null;
                    }
                    
                    adminContext.CreateFlightSchedule(flightSchedule);
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(scheduleViewModel);
            }
        }

        public FlightSchedule mapViewModeltoSchedule(ScheduleViewModel viewModel)
        {
            FlightSchedule schedule = new FlightSchedule();
            schedule.FlightNumber = viewModel.FlightNumber;
            schedule.RouteId = viewModel.RouteId;
            if (viewModel.AircraftId == null)
            {
                schedule.AircraftId = null;
            }
            else
            {
                schedule.AircraftId = Convert.ToInt32(viewModel.AircraftId);
            }
            schedule.DepartureDateTime = viewModel.DepartureDateTime;
            schedule.EconomyClassPrice = viewModel.EconomyClassPrice;
            schedule.BusinessClassPrice = viewModel.BusinessClassPrice;
            schedule.Status = viewModel.Status;
            return schedule;
        }

        public ActionResult AssignPersonnel()
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

            ViewData["ScheduleList"] = getUnassignedScheduleList();
            ViewData["ScheduleStaff"] = getUnassignedScheduleStaffList();

            return View();
        }

        private List<SelectListItem> getUnassignedScheduleList()
        {
            List<FlightSchedule> idList = adminContext.getAllUnassignedSchedule();
            List<SelectListItem> scheduleList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "Please select ..."
                }
            };

            foreach (FlightSchedule schedule in idList)
            {

                scheduleList.Add(new SelectListItem
                {
                    Value = schedule.ScheduleId.ToString(),
                    Text = schedule.ScheduleId.ToString() + " (Flight number:" + schedule.FlightNumber.ToString() + ")"
                });
            }
           
            return scheduleList;
        }

        private List<AssignPersonnelViewModel> getUnassignedScheduleStaffList()
        {
            List<AssignPersonnelViewModel> fsList = new List<AssignPersonnelViewModel>();
            List<FlightSchedule> scheduleList = adminContext.getAllUnassignedSchedule();
            List<PersonnelViewModel> personnelViewModels = mapPersonneltoSchedule();

            foreach (FlightSchedule schedule in scheduleList)
            {
                List<Staff> availableStaffList = new List<Staff>();
                foreach (PersonnelViewModel personnel in personnelViewModels)
                {
                    bool isAvailable = true;
                    if (personnel.Status == "Inactive")
                    {
                        isAvailable = false;
                    }
                    foreach (FlightSchedule flightSchedule in personnel.flightScheduleList)
                    {
                        if (Convert.ToDateTime(flightSchedule.DepartureDateTime).Date == Convert.ToDateTime(schedule.DepartureDateTime).Date)
                        {
                            isAvailable = false;
                        }
                    }

                    if (isAvailable)
                    {
                        Staff staff = adminContext.GetSpecificStaffByID(personnel.StaffId);
                        availableStaffList.Add(staff);
                    }
                }
                AssignPersonnelViewModel model = new AssignPersonnelViewModel
                {
                    flightSchedule = schedule,
                    personnelList = availableStaffList
                };
                fsList.Add(model);
            }


            return fsList;
        }

        [HttpPost]
        public ActionResult AssignPersonnel(SchedulePersonnel schedulePersonnel)
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

            if (ModelState.IsValid && !IsStaffRepeated(schedulePersonnel))
            {
                if (schedulePersonnel != null && schedulePersonnel.StaffIDList.Count == 6)
                {
                    Console.WriteLine(schedulePersonnel.ScheduleID);
                    foreach (int staff in schedulePersonnel.StaffIDList)
                    {
                        Console.WriteLine(staff);
                    }
                    adminContext.AssignFlightCrewsToSchedule(schedulePersonnel);

                    return RedirectToAction("Index", "Admin");
                }
                
            }
                
            ViewData["ScheduleList"] = getUnassignedScheduleList();
            ViewData["ScheduleStaff"] = getUnassignedScheduleStaffList();
            TempData["Alert"] = "There are duplicate personnels in the schedule!";

            return RedirectToAction("AssignPersonnel","Admin");
            
        }

        public bool IsStaffRepeated(SchedulePersonnel schedulePersonnel)
        {
            bool isStaffRepeated = false;

            HashSet<int> set = new HashSet<int>();

            foreach (int id in schedulePersonnel.StaffIDList)
            {
                if (set.Contains(id))
                {
                    return true;
                }
                set.Add(id);
            }

            return isStaffRepeated;
        }
    }
}