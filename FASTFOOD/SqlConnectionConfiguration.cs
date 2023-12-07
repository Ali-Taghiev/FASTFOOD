using FASTFOOD.Models;
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
        public List<Table> GetTables()
        {
            List<Table> tables = new List<Table>();

            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM tables", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Table table = new Table
                        {
                            Id = reader.GetInt64(0),
                            Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                            ActualServiceId = reader.IsDBNull(2) ? (long?)null : reader.GetInt64(2)
                        };




                        tables.Add(table);
                    }
                }
            }

            return tables;
        }




        public List<Service> GetServices()
        {
            List<Service> services = new List<Service>();

            using (SqlConnection connection = GetSqlConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Services", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Service service = new Service
                        {
                            Id = reader.GetInt64(0),
                            Start = reader.GetDateTime(1),
                            End = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2)
                        };

                        services.Add(service);
                    }
                }
            }

            return services;
        }
    }
   

}
