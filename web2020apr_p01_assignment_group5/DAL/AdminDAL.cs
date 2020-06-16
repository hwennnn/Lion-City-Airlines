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
                    StaffId = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1), //1: 2nd column
                    Vocation = reader.GetString(4), //5: 6th column

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
                    schedule.RouteID = reader.GetInt32(2);
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

    }
}
