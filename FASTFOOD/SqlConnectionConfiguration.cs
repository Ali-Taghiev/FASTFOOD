using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASTFOOD
{
    public class SqlConnectionConfiguration
    {
        // Connection details
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        // Constructor to set connection details
        public SqlConnectionConfiguration()
        {
            // Set your SQL Server connection details here
            this.Server = "LAPTOP-44";
            this.Database = "FastFoodDB";
            this.Username = "Ali";
            this.Password = "12345";
        }

        // Method to create and return a SQL Server connection string
        public string GetConnectionString()
        {
            return $"Server={Server};Database={Database};Integrated Security=true;TrustServerCertificate=true;";

        }

        // Method to establish a SqlConnection using the connection string
        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
