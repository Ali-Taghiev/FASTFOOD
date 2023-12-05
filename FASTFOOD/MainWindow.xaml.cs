using FASTFOOD.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FASTFOOD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SqlConnectionConfiguration sqlConfig = new SqlConnectionConfiguration();

            // Get connection string
            string connectionString = sqlConfig.GetConnectionString();
         

            
        }

        private void Login_btnClick(object sender, RoutedEventArgs e)
        {
            string username = txtboxUserName.Text;
            string password = txtboxPassword.Text;

            try
            {
                SqlConnectionConfiguration sqlConfig = new SqlConnectionConfiguration();

                using (SqlConnection con = sqlConfig.GetSqlConnection())
                {
                    con.Open();

                    string query = "SELECT username, password FROM Users WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string storedPassword = dr["password"].ToString();

                                // Check if the provided password matches the stored password
                                if (VerifyPassword(password, storedPassword))
                                {
                                    // Successful login
                                    DashBoard form = new DashBoard();
                                    form.Show();
                                }
                                else
                                {
                                    // Invalid password, handle accordingly
                                }
                            }
                            else
                            {
                                // Invalid username, handle accordingly
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred: {exc.ToString()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // Method to verify password (you should implement your own secure password hashing mechanism)
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Implement your password verification logic here (e.g., using a secure hashing algorithm)
            // For demonstration purposes, a simple string comparison is used here.
            return enteredPassword == storedPassword;
        }

    }
}
