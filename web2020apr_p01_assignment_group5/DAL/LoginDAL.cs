using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace web2020apr_p01_assignment_group5.DAL
{
    public class LoginDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public LoginDAL()
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

        public bool checkStaff(string email, string password)
        {
            bool check = false;

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
                    if (reader.GetString(6) == password && reader.GetString(4) == "Administrator")
                    {
                        Console.WriteLine(reader.GetString(6));
                        check = true;
                    } 
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return check;
        }

        public bool checkCustomer(string email, string password)
        {
            bool check = false;

            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Customer WHERE EmailAddr = @email";

            cmd.Parameters.AddWithValue("@email", email);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetString(6) == password)
                    {
                        check = true;
                    }
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return check;
        }


    }
}
