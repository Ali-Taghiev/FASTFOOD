using FASTFOOD.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
            Login();
        }
        private void Login()
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
                                string storedPasswordHash = dr["password"].ToString();

                                if (VerifyPassword(password, storedPasswordHash))
                                {
                                    DashBoard form = new DashBoard();
                                    form.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            string enteredPasswordHash = ComputeSha256Hash(enteredPassword);
            return string.Equals(enteredPasswordHash, storedPasswordHash, StringComparison.OrdinalIgnoreCase);
        }

        private string ComputeSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


        
        private void Login_EnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }

        }
    }
}