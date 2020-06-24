using Microsoft.Extensions.Configuration;
using System;
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
            cmd.Parameters.AddWithValue("@nationality", customer.Nationality);
            cmd.Parameters.AddWithValue("@birthDate", customer.BirthDate);
            cmd.Parameters.AddWithValue("@telNo", customer.TelNo);
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

        public bool IsEmailExist(string email, int CustomerID)
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
                    if (reader.GetInt32(0) != CustomerID)
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
            //retrieves all attributes of a staff record. 
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
    }
}
