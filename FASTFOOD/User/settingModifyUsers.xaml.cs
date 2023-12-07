
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace FASTFOOD.User
{
    public partial class settingModifyUsers : Window
    {
        private string userOriginal;

        public settingModifyUsers(string username)
        {
            InitializeComponent();

            this.userOriginal = username;
            usernameOriginal.Text = this.userOriginal;

            try
            {
               SqlConnectionConfiguration Config = new SqlConnectionConfiguration();
                string connectionString = Config.GetConnectionString();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT username, name, email FROM Users WHERE username = @userOriginal;";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userOriginal", userOriginal);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                userUsername.Text = dr.GetString(0);
                                userName.Text = dr.GetString(1);
                                userMail.Text = dr.GetString(2);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading the user: \n{exc.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

        }

        private void userModify_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text != "" && userUsername.Text != "" && userMail.Text != "")
            {
                try
                {
                    SqlConnectionConfiguration config = new SqlConnectionConfiguration();
                    string connectionString = config.GetConnectionString();
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string query;
                        if (userPassword.Password == "")
                        {
                            query = "UPDATE Users SET name = @name, username = @username, email = @email WHERE username = @userOriginal;";
                        }
                        else
                        {
                            query = "UPDATE Users SET name = @name, username = @username, password = @password, email = @email WHERE username = @userOriginal;";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@name", userName.Text);
                            cmd.Parameters.AddWithValue("@username", userUsername.Text);
                            cmd.Parameters.AddWithValue("@password", GetSHA256Hash(userPassword.Password));
                            cmd.Parameters.AddWithValue("@email", userMail.Text);
                            cmd.Parameters.AddWithValue("@userOriginal", userOriginal);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                               
                                this.Close();
                                settings settingsPage = new settings();
                                settingsPage.RefreshDataGridView();

                            }
                            else
                            {
                                MessageBox.Show("Error modifying the user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show($"An error occurred while modifying the user: \n{exc.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You must complete all fields.", "Incomplete Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

    }
}
