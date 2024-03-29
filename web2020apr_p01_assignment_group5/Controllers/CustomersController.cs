﻿using System;
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
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }

            return View();
        }

        // POST: Customer/CreateCustomerProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomerProfile(Customer customer)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }

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
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }

            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
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
                viewAirTickets.DepartureDateTime = Convert.ToDateTime(schedule.DepartureDateTime);
                viewAirTickets.ArrivalCity = flightRoutes.ArrivalCity;
                viewAirTickets.ArrivalCountry = flightRoutes.ArrivalCountry;
                viewAirTickets.ArrivalDateTime = Convert.ToDateTime(schedule.ArrivalDateTime);
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
            viewAirTicketsBooked.DepartureDateTime = Convert.ToDateTime(flightSchedule.DepartureDateTime);
            viewAirTicketsBooked.ArrivalCity = flightRoute.ArrivalCity;
            viewAirTicketsBooked.ArrivalCountry = flightRoute.ArrivalCountry;
            viewAirTicketsBooked.ArrivalDateTime = Convert.ToDateTime(flightSchedule.ArrivalDateTime);
            return viewAirTicketsBooked;
        }

        public ActionResult BookAirTicketsPersonalDetails(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }

            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                TempData["alert"] = "You need to sign in as a customer first!";
                return RedirectToAction("Login", "Home");
            }

            FlightSchedule schedule = adminContext.getSpecificSchedule(id);
            if (schedule.ScheduleId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            FlightRoute route = adminContext.getSpecificRoute(schedule.RouteId);
            Booking booking = new Booking();
            booking.ScheduleId = id;
            booking.AmtPayable = schedule.EconomyClassPrice;

            ViewData["Schedule"] = schedule;
            ViewData["Route"] = route;
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAirTicketsPersonalDetails(Booking booking)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }

            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                TempData["alert"] = "You need to sign in as a customer first!";
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                if (customerContext.checkpassportnumber(booking.ScheduleId, booking.PassportNumber))
                {
                    TempData["alert"] = "You have already made this booking. You are not allow to book this booking again!";
                    return RedirectToAction("ViewAvailableFlight", "Customers");
                }
                else
                {
                    TempData["alert"] = "Your Booking for " + booking.PassengerName + " is Successful. Thank You for visting Lion City Airlines!";
                    booking.CustomerId = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
                    //Add booking record to database
                    customerContext.BookTickets(booking);
                }       
            }
            else
            {
                TempData["alert"] = "Your Booking is Unsuccessful, Please Try Again!";
                //Input validation fails, return to the BookAirTicketsPersonalDetails.cshtml view
                //to display error message
                return View(booking);
            }
            if (booking.IsNextPassenger)
            {
                return RedirectToAction("BookAirTicketsPersonalDetails", "Customers", booking.ScheduleId);
            }
            else
            {
                return RedirectToAction("ViewAirTicketsBooked", "Customers");
            }   
        }
        public ActionResult SelectedFlightSchedule(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }

            if (id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<FlightSchedule> selectedflightschedule = adminContext.getOpenedSchedulefromRouteID(id);
            
            return View(selectedflightschedule);
        }
        public ActionResult ViewAvailableFlight()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "Admin");
            }

            List<FlightRoute> ViewFlight = adminContext.getAllFlightRoute();
            return View(ViewFlight);
        }
    }
}