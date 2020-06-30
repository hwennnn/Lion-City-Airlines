﻿using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using web2020apr_p01_assignment_group5.Models;

namespace web2020apr_p01_assignment_group5.DAL
{
    public class AdminDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public AdminDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "Air_Flights_DBConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public Staff GetSpecificStaffByEmail(string email)
        {
            Staff staff = new Staff();

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff WHERE EmailAddr = @email";
            cmd.Parameters.AddWithValue("@email", email);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    staff.StaffId = reader.GetInt32(0);
                    staff.StaffName = reader.GetString(1);
                    staff.Gender = reader.GetString(2)[0];
                    staff.DateEmployed = reader.GetDateTime(3);
                    staff.Vocation = reader.GetString(4);
                    staff.Email = reader.GetString(5);
                    staff.Status = reader.GetString(7);
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return staff;
        }

        public Staff GetSpecificStaffByID(int id)
        {
            Staff staff = new Staff();

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffID = @staffID";
            cmd.Parameters.AddWithValue("@staffID", id);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    staff.StaffId = reader.GetInt32(0);
                    staff.StaffName = reader.GetString(1);
                    staff.Gender = reader.GetString(2)[0];
                    staff.DateEmployed = reader.GetDateTime(3);
                    staff.Vocation = reader.GetString(4);
                    staff.Email = reader.GetString(5);
                    staff.Status = reader.GetString(7);
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return staff;
        }

        public List<Staff> getAllStaff()
        {
            List<Staff> staffList = new List<Staff>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffId = reader.GetInt32(0),
                    StaffName = reader.GetString(1),
                    Gender = reader.GetString(2)[0],
                    DateEmployed = reader.GetDateTime(3),
                    Vocation = reader.GetString(4),
                    Email = reader.GetString(5),
                    Status = reader.GetString(7)

            });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return staffList;
        }

        public FlightSchedule getSpecificSchedule(int scheduleID)
        {
            FlightSchedule schedule = new FlightSchedule();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE ScheduleID = @ScheduleID";
            cmd.Parameters.AddWithValue("@ScheduleID", scheduleID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    schedule.ScheduleId = reader.GetInt32(0);
                    schedule.FlightNumber = reader.GetString(1);
                    schedule.RouteId = reader.GetInt32(2);
                    schedule.DepartureDateTime = reader.GetDateTime(4);
                    schedule.ArrivalDateTime = reader.GetDateTime(5);
                    schedule.Status = reader.GetString(8);
                }
            }
         
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return schedule;
        }

        public List<FlightSchedule> getAllFlightSchedule()
        {
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule ORDER BY ScheduleID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                scheduleList.Add(
                new FlightSchedule
                {
                    ScheduleId = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    RouteId = reader.GetInt32(2),
                    AircraftId = reader.GetInt32(3),
                    DepartureDateTime = reader.GetDateTime(4),
                    ArrivalDateTime = reader.GetDateTime(5),
                    EconomyClassPrice = Convert.ToDouble(reader.GetDecimal(6)),
                    BusinessClassPrice = Convert.ToDouble(reader.GetDecimal(7)),
                    Status = reader.GetString(8),

            });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }
        public FlightRoute getSpecificRoute (int routeID)
        {
            FlightRoute route = new FlightRoute();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightRoute WHERE RouteID = @RouteID";
            cmd.Parameters.AddWithValue("@RouteID", routeID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    route.RouteId = reader.GetInt32(0);
                    route.DepartureCity = reader.GetString(1);
                    route.DepartureCountry = reader.GetString(2);
                    route.ArrivalCity = reader.GetString(3);
                    route.ArrivalCountry = reader.GetString(4);
                    route.FlightDuration = reader.GetInt32(5);
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return route;
        }

        public bool IsRouteExist(FlightRoute route)
        {
            //Assign a boolean for return if FlightRoute Exists
            bool routeFound = false;

            //Create sql command
            SqlCommand cmd = conn.CreateCommand();
            //Set SQL Command Text
            cmd.CommandText = @"SELECT * FROM FlightRoute WHERE DepartureCity = @selectedDepartureCity
                                AND DepartureCountry = @selectedDepartureCountry
                                AND ArrivalCity = @selectedArrivalCity
                                AND ArrivalCountry = @selectedArrivalCountry";
            //Set parameters for SQL Command
            cmd.Parameters.AddWithValue("@selectedDepartureCity", route.DepartureCity);
            cmd.Parameters.AddWithValue("@selectedDepartureCountry", route.DepartureCountry);
            cmd.Parameters.AddWithValue("@selectedArrivalCity", route.ArrivalCity);
            cmd.Parameters.AddWithValue("@selectedArrivalCountry", route.ArrivalCountry);
            //Open connection to DB
            conn.Open();
            //Read SQL data using command text
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {//If record exists
                while (reader.Read())
                {
                    routeFound = true;
                }
            }
            else
            {//If record does not exist
                routeFound = false;
            }
            reader.Close();
            conn.Close();
            return routeFound;
        }

        public List<FlightCrew> getSpecificFlightCrew(int staffID)
        {
            List<FlightCrew> flightCrewList = new List<FlightCrew>();

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightCrew WHERE StaffID = @StaffID";
            cmd.Parameters.AddWithValue("@StaffID", staffID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    flightCrewList.Add(
                    new FlightCrew
                    {
                        ScheduleID = reader.GetInt32(0),
                        StaffID = reader.GetInt32(1),
                        Role = reader.GetString(2)

                    });
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return flightCrewList;
        }

        public List<FlightCrew> getAllFlightCrew()
        {
            List<FlightCrew> flightCrewList = new List<FlightCrew>();

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightCrew ORDER BY ScheduleID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                flightCrewList.Add(
                new FlightCrew
                {
                    ScheduleID = reader.GetInt32(0),
                    StaffID = reader.GetInt32(1), 
                    Role = reader.GetString(2)

                });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return flightCrewList;
        }

        public int CreatePersonnel(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Staff (StaffName, Gender, DateEmployed, Vocation,
                                EmailAddr, Password, Status)
                                OUTPUT INSERTED.StaffID
                                VALUES(@name, @gender, @dateEmployed, @vocation,
                                @email, @password, @status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property
            cmd.Parameters.AddWithValue("@name", staff.StaffName);
            cmd.Parameters.AddWithValue("@gender", staff.Gender);
            cmd.Parameters.AddWithValue("@dateEmployed", staff.DateEmployed);
            cmd.Parameters.AddWithValue("@vocation", staff.Vocation);
            cmd.Parameters.AddWithValue("@email", staff.Email);
            cmd.Parameters.AddWithValue("@password", "p@55Staff");
            cmd.Parameters.AddWithValue("@status", "Active");
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            staff.StaffId = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return staff.StaffId;
        }

        public bool IsEmailExist(string email)
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a customer record with the email address to be validated 
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Staff 
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and excute the SQL statement 
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    emailFound = true;
                }
            }
            else
            { //No record 
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();
            return emailFound;
        }
        
        public int CreateFlightRoute(FlightRoute flightRoute)
        {
            //Create SqlCommand from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated FlightRoute ID after insertion
            cmd.CommandText = @"INSERT INTO FlightRoute (DepartureCity, DepartureCountry,
                                ArrivalCity, ArrivalCountry, FlightDuration)
                                OUTPUT INSERTED.RouteID
                                VALUES(@departureCity, @departureCountry,
                                @arrivalCity, @arrivalCountry, @flightDuration)";
            //Defining parameters to be inserted
            cmd.Parameters.AddWithValue("@departureCity", flightRoute.DepartureCity);
            cmd.Parameters.AddWithValue("@departureCountry", flightRoute.DepartureCountry);
            cmd.Parameters.AddWithValue("@arrivalCity", flightRoute.ArrivalCity);
            cmd.Parameters.AddWithValue("@arrivalCountry", flightRoute.ArrivalCountry);
            cmd.Parameters.AddWithValue("@flightDuration", flightRoute.FlightDuration);
            //Opening connection to database
            conn.Open();
            //Execute Scalar to retrieve inserted Route ID
            flightRoute.RouteId = (int)cmd.ExecuteScalar();
            //Close the connection
            conn.Close();
            //Return ID if successful
            return flightRoute.RouteId;
        }

        public void updatePersonnelStatus(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Status=@status 
                                WHERE StaffID = @selectedStaffID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.

            if (staff.Status.Equals("Active"))
            {
                cmd.Parameters.AddWithValue("@status", "Inactive");
            }
            else
            {
                cmd.Parameters.AddWithValue("@status", "Active");
            }

            cmd.Parameters.AddWithValue("@selectedStaffID", staff.StaffId);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

        }

        public void updateFlightScheduleStatus(FlightSchedule schedule, string status)
        {
            //Create SQL Command
            SqlCommand cmd = conn.CreateCommand();
            //Create UPDATE SQL Statement for FlightSchedule
            cmd.CommandText = @"UPDATE FlightSchedule SET Status=@status
                                WHERE ScheduleID = @selectedScheduleID";
            //Define parameters used in SQL
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("selectedScheduleID", schedule.ScheduleId);

            //Open connection to database
            conn.Open();
            //Use ExequteNonQuery for UPDATE
            cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
        }

    }
}
