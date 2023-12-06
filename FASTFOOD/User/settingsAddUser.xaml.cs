using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace FASTFOOD.User
{
    /// <summary>
    /// Interaction logic for settingsAddUser.xaml
    /// </summary>
    public partial class settingsAddUser : Window
    {
        public settingsAddUser()
        {
            InitializeComponent();
        }

        private void userCreate_Click(object sender, RoutedEventArgs e)
        {
            

            if (userName.Text != "" && userUsername.Text != "" && userMail.Text != "" && userPassword.Password != "")
            {
                SqlConnectionConfiguration config = new SqlConnectionConfiguration();
                string connectionString = config.GetConnectionString();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        string hashedPassword = GetSHA256Hash(userPassword.Password);

                        string query = "INSERT INTO Users (name, username, password, email) VALUES (@name, @username, @password, @email)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@name", userName.Text);
                            cmd.Parameters.AddWithValue("@username", userUsername.Text);
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                            cmd.Parameters.AddWithValue("@email", userMail.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Error adding user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.", "Incomplete Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        
    }
}
