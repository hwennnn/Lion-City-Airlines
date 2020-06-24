using System;
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
                    DepartureDateTime = reader.GetDateTime(4),
                    ArrivalDateTime = reader.GetDateTime(5),
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
            //is retrieved from respective class's property.
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

    }
}
