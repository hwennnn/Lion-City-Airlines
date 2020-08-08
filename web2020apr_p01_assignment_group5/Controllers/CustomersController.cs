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
    public class CustomersController : Controller
    {
        private CustomerDAL customerContext = new CustomerDAL();
        private AdminDAL adminContext = new AdminDAL();
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
                TempData["alert"] = "Your Account Password is p@55Cust";
                //Redirect user to Home/Login view
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //Input validation fails, return to the CreateCustomerProfile.cshtml view
                //to display error message
                TempData["alert"] = "Error creating an account. Sending back to Create Customer Profile.";
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
                    if(changePassword.CurrentPassword == changePassword.NewPassword)
                    {
                        return RedirectToAction("ChangePassword");
                    }
                    else
                    {
                        customerContext.ChangePassword(customer, changePassword);
                        TempData["alert"] = "Password has been changed successfully!";
                        return RedirectToAction("Index");
                    }                   
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct.");
                }
            }
            return View();
        }

        public List<ViewAirTicketsBooked> BookingToViewBooking(int? CustomerId)
        {
            List<ViewAirTicketsBooked> BookingModelList = new List<ViewAirTicketsBooked>();
            List<Booking> BookingList = customerContext.GetAllbooking(CustomerId);
            List<FlightSchedule> scheduleList = adminContext.getAllFlightSchedule();          

            foreach (Booking Booking in BookingList) 
            {
                FlightSchedule schedule = scheduleList.First(b => b.ScheduleId == Booking.ScheduleId);
                FlightRoute flightRoutes = adminContext.getSpecificRoute(schedule.RouteId);
                ViewAirTicketsBooked viewAirTickets = new ViewAirTicketsBooked();
                viewAirTickets.BookingId = Booking.BookingId;
                viewAirTickets.PassengerName = Booking.PassengerName;
                viewAirTickets.PassportNumber = Booking.PassportNumber;
                viewAirTickets.Nationality = Booking.Nationality;
                viewAirTickets.SeatClass = Booking.SeatClass;
                viewAirTickets.AmtPayable = Booking.AmtPayable;
                viewAirTickets.Remarks = Booking.Remarks;
                viewAirTickets.DateTimeCreated = Booking.DateTimeCreated;
                viewAirTickets.FlightNumber = schedule.FlightNumber;
                viewAirTickets.DepartureCity = flightRoutes.DepartureCity;
                viewAirTickets.DepartureCountry = flightRoutes.DepartureCountry;
                viewAirTickets.DepartureDateTime = schedule.DepartureDateTime;
                viewAirTickets.ArrivalCity = flightRoutes.ArrivalCity;
                viewAirTickets.ArrivalCountry = flightRoutes.ArrivalCountry;
                viewAirTickets.ArrivalDateTime = schedule.ArrivalDateTime;
                BookingModelList.Add(viewAirTickets);
            }
            return BookingModelList;
        }

        public ActionResult ViewAirTicketsBooked()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            int? id = HttpContext.Session.GetInt32("CustomerID");
            return View(BookingToViewBooking(id));
        }

        public ActionResult ViewAirTicketsBookedDetails(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetSpecificDetails(id));
        }

        public ViewAirTicketsBooked GetSpecificDetails(int? BookingId)
        {
            Booking booking = customerContext.GetViewAirTicketsBookedDetails(BookingId);
            ViewAirTicketsBooked viewAirTicketsBooked = new ViewAirTicketsBooked();
            FlightSchedule flightSchedule = adminContext.getSpecificSchedule(booking.ScheduleId);
            FlightRoute flightRoute = adminContext.getSpecificRoute(flightSchedule.RouteId);
            viewAirTicketsBooked.BookingId = booking.BookingId;
            viewAirTicketsBooked.PassengerName = booking.PassengerName;
            viewAirTicketsBooked.PassportNumber = booking.PassportNumber;
            viewAirTicketsBooked.Nationality = booking.Nationality;
            viewAirTicketsBooked.SeatClass = booking.SeatClass;
            viewAirTicketsBooked.AmtPayable = booking.AmtPayable;
            viewAirTicketsBooked.Remarks = booking.Remarks;
            viewAirTicketsBooked.DateTimeCreated = booking.DateTimeCreated;
            viewAirTicketsBooked.FlightNumber = flightSchedule.FlightNumber;
            viewAirTicketsBooked.DepartureCity = flightRoute.DepartureCity;
            viewAirTicketsBooked.DepartureCountry = flightRoute.DepartureCountry;
            viewAirTicketsBooked.DepartureDateTime = flightSchedule.DepartureDateTime;
            viewAirTicketsBooked.ArrivalCity = flightRoute.ArrivalCity;
            viewAirTicketsBooked.ArrivalCountry = flightRoute.ArrivalCountry;
            viewAirTicketsBooked.ArrivalDateTime = flightSchedule.ArrivalDateTime;
            return viewAirTicketsBooked;
        }
        public ActionResult BookAirTickets(string? departure, string? arrival)
        {
            if (departure == null && arrival == null)
            {
                if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
                {
                    return RedirectToAction("Index", "Home");
                }
                List<FlightSchedule> flightSchedules = new List<FlightSchedule>();
                flightSchedules = adminContext.getAllFlightSchedule();
                return View(flightSchedules);
            }
            else
            {
                FlightRoute flightRoute = adminContext.searchcountry(departure, arrival);
                List<FlightSchedule> flightSchedules = new List<FlightSchedule>();
                flightSchedules = adminContext.getschedulefromRouteID(flightRoute.RouteId);
                return View(flightSchedules);
            }
        }

        public ActionResult BookAirTicketsPersonalDetails(int id)
        {
            Booking booking = new Booking();
            booking.ScheduleId = id;
            ViewData["Schedule"] = adminContext.getSpecificSchedule(id);
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAirTicketsPersonalDetails(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
                //Add booking record to database
                customerContext.BookTickets(booking);
                //Redirect user to Home/Index view
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Input validation fails, return to the BookAirTicketsPersonalDetails.cshtml view
                //to display error message
                return View(booking);
            }
        }
        public ActionResult SelectedFlightSchedule(int id)
        {
            List<FlightSchedule> selectedflightschedule = new List<FlightSchedule>();
            selectedflightschedule = adminContext.getschedulefromRouteID(id);
            return View(selectedflightschedule);
        }
        public ActionResult ViewAvailableFlight()
        {
            List<FlightRoute> ViewFlight = adminContext.getAllFlightRoute();
            return View(ViewFlight);
        }
    }
}