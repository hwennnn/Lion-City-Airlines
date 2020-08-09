using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using web2020apr_p01_assignment_group5.Models;
using Microsoft.AspNetCore.Routing;

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
                    staff.Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char)0;
                    staff.DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                    staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;
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
                    staff.Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char)0;
                    staff.DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                    staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;
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
                    Gender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char)0,
                    DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                    Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null,
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
                    schedule.AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null;
                    schedule.DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;
                    schedule.ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;
                    schedule.EconomyClassPrice = Convert.ToDouble(reader.GetDecimal(6));
                    schedule.BusinessClassPrice = Convert.ToDouble(reader.GetDecimal(7));
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
                    AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null,
                    DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
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

        public List<FlightSchedule> getSpecificScheduleList(int id)
        {
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify SELECT Statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE RouteID = @routeId ORDER BY ScheduleID";
            //Add parameters for RouteID
            cmd.Parameters.AddWithValue("@routeId", id);
            //Open Database Connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    scheduleList.Add(
                    new FlightSchedule
                    {
                        ScheduleId = reader.GetInt32(0),
                        FlightNumber = reader.GetString(1),
                        RouteId = reader.GetInt32(2),
                        AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null,
                        DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                        ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                        EconomyClassPrice = Convert.ToDouble(reader.GetDecimal(6)),
                        BusinessClassPrice = Convert.ToDouble(reader.GetDecimal(7)),
                        Status = reader.GetString(8),

                    });
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }

        public FlightRoute getSpecificRoute(int routeID)
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
                    route.FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null;
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return route;
        }

        public List<FlightRoute> getAllFlightRoute()
        {
            List<FlightRoute> routeList = new List<FlightRoute>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightRoute ORDER BY RouteID";
            //Open database Connection
            conn.Open();
            //Start the reader to retrieve data
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                routeList.Add(
                new FlightRoute
                {
                    RouteId = reader.GetInt32(0),
                    DepartureCity = reader.GetString(1),
                    DepartureCountry = reader.GetString(2),
                    ArrivalCity = reader.GetString(3),
                    ArrivalCountry = reader.GetString(4),
                    FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null,
                });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return routeList;

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

            if (staff.Gender.Equals('N'))
            {
                cmd.Parameters.AddWithValue("@gender", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@gender", staff.Gender);
            }

            if (!staff.DateEmployed.HasValue)
            {
                cmd.Parameters.AddWithValue("@dateEmployed", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dateEmployed", staff.DateEmployed);
            }

            if (string.IsNullOrEmpty(staff.Vocation))
            {
                cmd.Parameters.AddWithValue("@vocation", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@vocation", staff.Vocation);
            }


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

        public void CreateFlightRoute(FlightRoute flightRoute)
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
            if (flightRoute.FlightDuration.HasValue)
            {
                cmd.Parameters.AddWithValue("@flightDuration", flightRoute.FlightDuration);
            }
            else
            {
                cmd.Parameters.AddWithValue("@flightDuration", DBNull.Value);
            }
            //Opening connection to database
            conn.Open();
            //Execute Scalar to retrieve inserted Route ID
            flightRoute.RouteId = (int)cmd.ExecuteScalar();
            //Close the connection
            conn.Close();

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

        public FlightRoute searchcountry(string departure, string arrival)
        {
            FlightRoute route = new FlightRoute();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightRoute WHERE DepartureCountry = @departureCountry AND ArrivalCountry = @arrivalCountry";
            cmd.Parameters.AddWithValue("@departureCountry", departure);
            cmd.Parameters.AddWithValue("@arrivalCountry", arrival);
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
                    route.FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null;
                }

            }

            return route;
        }

        public List<Aircraft> getAllAircraft()
        {
            List<Aircraft> aircraftList = new List<Aircraft>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM Aircraft ORDER BY AircraftID";
            //Open database Connection
            conn.Open();
            //Start the reader to retrieve data
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                aircraftList.Add(
                new Aircraft
                {
                    AircraftId = reader.GetInt32(0),
                    MakeModel = reader.GetString(1),
                    NumEconomySeat = reader.GetInt32(2),
                    NumBusinessSeat = reader.GetInt32(3),
                    DateLastMaintenance = reader.GetDateTime(4),
                    Status = reader.GetString(5),
                });
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return aircraftList;
        }
        public List<FlightSchedule> getschedulefromRouteID(int routeid)
        {
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE RouteID = @routeID";
            cmd.Parameters.AddWithValue("@routeID", routeid);
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
                    AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null,
                    DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                    EconomyClassPrice = Convert.ToDouble(reader.GetDecimal(6)),
                    BusinessClassPrice = Convert.ToDouble(reader.GetDecimal(7)),
                    Status = reader.GetString(8),
                });
            }
            reader.Close();
            conn.Close();

            return scheduleList;

        }

        public List<FlightSchedule> getOpenedSchedulefromRouteID(int routeid)
        {
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE RouteID = @routeID AND Status = @status";
            cmd.Parameters.AddWithValue("@routeID", routeid);
            cmd.Parameters.AddWithValue("@status", "Opened");
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DateTime? time = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;
                if (time != null && time > DateTime.Today)
                {
                    scheduleList.Add(
                    new FlightSchedule
                    {
                        ScheduleId = reader.GetInt32(0),
                        FlightNumber = reader.GetString(1),
                        RouteId = reader.GetInt32(2),
                        AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null,
                        DepartureDateTime = time,
                        ArrivalDateTime = reader.GetDateTime(5),
                        EconomyClassPrice = Convert.ToDouble(reader.GetDecimal(6)),
                        BusinessClassPrice = Convert.ToDouble(reader.GetDecimal(7)),
                        Status = reader.GetString(8),
                    });
                }
            }
            reader.Close();
            conn.Close();

            return scheduleList;

        }

        public void CreateFlightSchedule(FlightSchedule flightSchedule)
        {
            //Create SqlCommand from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated FlightRoute ID after insertion
            cmd.CommandText = @"INSERT INTO FlightSchedule (FlightNumber, RouteID, AircraftID,
                                DepartureDateTime, ArrivalDateTime, EconomyClassPrice, BusinessClassPrice, Status)
                                OUTPUT INSERTED.ScheduleID
                                VALUES(@FlightNumber, @RouteID,
                                @AircraftID, @DepartureDateTime, @ArrivalDateTime, @EconomyClassPrice, @BusinessClassPrice,
                                @Status)";
            //Defining parameters to be inserted
            cmd.Parameters.AddWithValue("@FlightNumber", flightSchedule.FlightNumber);
            cmd.Parameters.AddWithValue("@RouteID", flightSchedule.RouteId);
            if (!flightSchedule.AircraftId.HasValue)
            {
                cmd.Parameters.AddWithValue("@AircraftID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AircraftID", flightSchedule.AircraftId.Value);
            }
            if (!flightSchedule.DepartureDateTime.HasValue)
            {
                cmd.Parameters.AddWithValue("@DepartureDateTime", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("DepartureDateTime", flightSchedule.DepartureDateTime);
            }
            if (!flightSchedule.ArrivalDateTime.HasValue)
            {
                cmd.Parameters.AddWithValue("@ArrivalDateTime", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ArrivalDateTime", flightSchedule.ArrivalDateTime);
            }
            cmd.Parameters.AddWithValue("@EconomyClassPrice", flightSchedule.EconomyClassPrice);
            cmd.Parameters.AddWithValue("@BusinessClassPrice", flightSchedule.BusinessClassPrice);
            cmd.Parameters.AddWithValue("@Status", "Opened");
            //Opening connection to database
            conn.Open();
            //Execute Scalar to retrieve inserted Route ID
            flightSchedule.ScheduleId = (int)cmd.ExecuteScalar();
            //Close the connection
            conn.Close();
        }

        public List<int> getAllAssignedScheduleID()
        {
            List<int> idList = new List<int>();
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT DISTINCT(ScheduleID) from FlightCrew";
            //Open database Connection
            conn.Open();
            //Start the reader to retrieve data
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idList.Add(reader.GetInt32(0));
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return idList;
        }

        public List<FlightSchedule> getAllUnassignedSchedule()
        {
            List<int> assignedScheduleIDList = getAllAssignedScheduleID();
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * from FlightSchedule WHERE NOT [Status] = 'Cancelled'";
            //Open database Connection
            conn.Open();
            //Start the reader to retrieve data
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime? departureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;
                if (departureDateTime != null)
                {
                    if (!assignedScheduleIDList.Contains(id) && departureDateTime > DateTime.Today && departureDateTime != null)
                    {
                        FlightSchedule schedule = new FlightSchedule();
                        schedule.ScheduleId = id;
                        schedule.FlightNumber = reader.GetString(1);
                        schedule.RouteId = reader.GetInt32(2);
                        schedule.AircraftId = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null;
                        schedule.DepartureDateTime = departureDateTime;
                        schedule.ArrivalDateTime = reader.GetDateTime(5);
                        schedule.Status = reader.GetString(8);

                        scheduleList.Add(schedule);
                    }
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }

        public void AssignFlightCrewsToSchedule(SchedulePersonnel schedulePersonnel)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO FlightCrew(ScheduleID,StaffID,Role)
                                VALUES(@ScheduleID, @StaffID, @Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property

            cmd.Parameters.AddWithValue("ScheduleID", schedulePersonnel.ScheduleID);
            //A connection to database must be opened before any operations made.
            conn.Open();

            List<int> staffIDList = schedulePersonnel.StaffIDList;
            List<string> staffRoleList = GetPersonnelRoleList();
            cmd.Parameters.AddWithValue("@StaffID", staffIDList[0]);
            cmd.Parameters.AddWithValue("@Role", staffRoleList[0]);
            cmd.ExecuteScalar();

            for (int i = 1; i < 6; i++)
            {
                cmd.Parameters["@StaffID"].Value = staffIDList[i];
                cmd.Parameters["@Role"].Value = staffRoleList[i];
                cmd.ExecuteScalar();
            }

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            //A connection should be closed after operations.
            conn.Close();
        }

        public List<string> GetPersonnelRoleList()
        {
            List<string> staffRoleList = new List<string>
            {
                "Flight Captain",
                "Second Pilot",
                "Cabin Crew Leader",
                "Flight Attendant",
                "Flight Attendant",
                "Flight Attendant"
            };

            return staffRoleList;
        }

        public bool IsFlightDurationNull(int rid)
        {
            //Assign a boolean for return if FlightDuration is/is not null
            bool duration = false;
            FlightRoute route = new FlightRoute();

            //Create sql command
            SqlCommand cmd = conn.CreateCommand();
            //Set SQL Command Text
            cmd.CommandText = @"SELECT * FROM FlightRoute WHERE RouteID = @selectedRouteID";
            //Set parameter for SQL Command
            cmd.Parameters.AddWithValue("@selectedRouteID", rid);
            //Open connection to DB
            conn.Open();
            //Read SQL data using command text
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                route.FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null;
            }
            if (route.FlightDuration == null)
            {//If duration is null
                duration = true;
            }
            else
            {//If duration has a value
                duration = false;
            }
            reader.Close();
            conn.Close();
            return duration;
        }

    }
}
