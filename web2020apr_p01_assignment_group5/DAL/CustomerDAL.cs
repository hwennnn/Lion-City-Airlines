using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using web2020apr_p01_assignment_group5.Models;

namespace web2020apr_p01_assignment_group5.DAL
{
    public class CustomerDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor 
        public CustomerDAL()
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
        public int Addcustomer(Customer customer)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will 
            //return the auto-generated CustomerID after insertion 
            cmd.CommandText = @"INSERT INTO Customer (CustomerName, Nationality, BirthDate, TelNo, 
                                EmailAddr) 
                                OUTPUT INSERTED.CustomerID 
                                VALUES(@customerName, @nationality, @birthDate, @telNo, 
                                @emailAddr)";
            //Define the parameters used in SQL statement, value for each parameter 
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
            if (string.IsNullOrEmpty(customer.Nationality))
            {
                cmd.Parameters.AddWithValue("@nationality", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@nationality", customer.Nationality);
            }

            if (!customer.BirthDate.HasValue)
            {
                cmd.Parameters.AddWithValue("@birthDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@birthDate", customer.BirthDate);
            }

            if (string.IsNullOrEmpty(customer.TelNo))
            {
                cmd.Parameters.AddWithValue("@telNo", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@telNo", "+65"+customer.TelNo);
            }
            cmd.Parameters.AddWithValue("@emailAddr", customer.EmailAddr);
            //A connection to database must be opened before any operations made. 
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated 
            //CustomerId after executing the INSERT SQL statement 
            customer.CustomerId = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations. 
            conn.Close();
            //Return id when no error occurs. 
            return customer.CustomerId;
        }

        public bool IsEmailExist(string email)
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a customer record with the email address to be validated 
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CustomerID FROM Customer 
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and excute the SQL statement 
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {

                    //The email address is used by another Customer 
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

        public Customer GetDetails(string EmailAddr)
        {
            Customer customer = new Customer();
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that 
            //retrieves all attributes of a customer record. 
            cmd.CommandText = @"SELECT * FROM Customer 
                                WHERE EmailAddr = @selectedCustomerID";
            //Define the parameter used in SQL statement, value for the 
            //parameter is retrieved from the method parameter “customerID”. 
            cmd.Parameters.AddWithValue("@selectedCustomerID", EmailAddr);

            //Open a database connection 
            conn.Open();
            //Execute SELCT SQL through a DataReader 
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database 
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader 
                    customer.CustomerId = reader.GetInt32(0);
                    customer.CustomerName = reader.GetString(1);
                    // (char) 0 - ASCII Code 0 - null value 
                    customer.Nationality = !reader.IsDBNull(2) ?
                                    reader.GetString(2) : null;
                    customer.BirthDate = !reader.IsDBNull(3) ?
                                    reader.GetDateTime(3) : (DateTime?)null;
                    customer.TelNo = !reader.IsDBNull(4) ?
                                    reader.GetString(4) : null;
                    customer.EmailAddr = reader.GetString(5);
                    customer.Password = reader.GetString(6);
                }
            }
            //Close data reader 
            reader.Close();
            //Close database connection
            conn.Close();

            return customer;
        }

        public int ChangePassword(Customer customer, ChangePassword changePassword)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement 
            cmd.CommandText = @"UPDATE Customer SET Password=@password
                                WHERE CustomerID = @selectedCustomerID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectedCustomerID", customer.CustomerId);
            cmd.Parameters.AddWithValue("@password", changePassword.NewPassword);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public List<Booking> GetAllbooking(int? CustomerId)
        {
            List<Booking> bookingList = new List<Booking>();
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that 
            cmd.CommandText = @"SELECT * FROM Booking WHERE CustomerID = @CustomerID ORDER BY BookingID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
            //Open a database connection 
            conn.Open();
            //Execute SELCT SQL through a DataReader 
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bookingList.Add(
                new Booking
                {
                    BookingId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    ScheduleId = reader.GetInt32(2),
                    PassengerName = reader.GetString(3),
                    PassportNumber = reader.GetString(4),
                    Nationality = reader.GetString(5),
                    SeatClass = reader.GetString(6),
                    AmtPayable = Convert.ToDouble(reader.GetDecimal(7)),
                    Remarks = !reader.IsDBNull(8) ? 
                        reader.GetString(8) : null,
                    DateTimeCreated =reader.GetDateTime(9)
                });
            }
            //Close data reader 
            reader.Close();
            //Close database connection
            conn.Close();

            return bookingList;
        }

        public Booking GetViewAirTicketsBookedDetails(int? BookingId)
        {
            Booking ViewDetails = new Booking();
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that 
            cmd.CommandText = @"SELECT * FROM Booking WHERE BookingID = @BookingID";
            cmd.Parameters.AddWithValue("@BookingID", BookingId);
            //Open a database connection 
            conn.Open();
            //Execute SELCT SQL through a DataReader 
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    ViewDetails.BookingId = reader.GetInt32(0);
                    ViewDetails.CustomerId = reader.GetInt32(1);
                    ViewDetails.ScheduleId = reader.GetInt32(2);
                    ViewDetails.PassengerName = reader.GetString(3);
                    ViewDetails.PassportNumber = reader.GetString(4);
                    ViewDetails.Nationality = reader.GetString(5);
                    ViewDetails.SeatClass = reader.GetString(6);
                    ViewDetails.AmtPayable = Convert.ToDouble(reader.GetDecimal(7));
                    ViewDetails.Remarks = !reader.IsDBNull(8) ?
                        reader.GetString(8) : null;
                    ViewDetails.DateTimeCreated = reader.GetDateTime(9);
                }
            }        
            //Close data reader 
            reader.Close();
            //Close database connection
            conn.Close();
            return ViewDetails;
        }

        public int BookTickets(Booking booking)
        {
            DateTime datetimecreated = DateTime.Now;
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Booking (CustomerID, ScheduleID, PassengerName, PassportNumber, Nationality, SeatClass, AmtPayable, Remarks, DateTimeCreated) 
                                OUTPUT INSERTED.BookingID 
                                VALUES(@customerID, @scheduleID, @passengerName, @passportNumber, @nationality, @seatclass, @amtPayable, 
                                @remarks, @dateTimeCreated)";
            //Define the parameters used in SQL statement, value for each parameter 
            cmd.Parameters.AddWithValue("@customerID", booking.CustomerId);
            cmd.Parameters.AddWithValue("@scheduleID", booking.ScheduleId);
            cmd.Parameters.AddWithValue("@passengerName", booking.PassengerName);
            cmd.Parameters.AddWithValue("@passportNumber", booking.PassportNumber);
            cmd.Parameters.AddWithValue("@nationality", booking.Nationality);
            cmd.Parameters.AddWithValue("@seatclass", booking.SeatClass);
            cmd.Parameters.AddWithValue("@amtPayable", booking.AmtPayable);
            if (string.IsNullOrEmpty(booking.Remarks))
            {
                cmd.Parameters.AddWithValue("@remarks", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@remarks", booking.Remarks);
            }
            cmd.Parameters.AddWithValue("@dateTimeCreated", datetimecreated);
            //A connection to database must be opened before any operations made. 
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated 
            booking.BookingId = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations. 
            conn.Close();
            return booking.BookingId;
        }
    }      
}
