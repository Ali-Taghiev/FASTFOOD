using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace FASTFOOD.User
{
  
    public partial class settings : Window
    {

        private string UsersDataGridSelected;
        public settings()
        {
            InitializeComponent();

            
            version.Text = version.Text + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (users.IsSelected)
            {
                GetFromUsers();
            }
        }

       



        private void DataGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the event was triggered by a user action
            if (e.AddedItems.Count > 0)
            {
                DataRowView row = (DataRowView)DataGridUsers.SelectedItem;
                if (row != null)
                {
                    userDelete.IsEnabled = true;
                    userModify.IsEnabled = true;
                    UsersDataGridSelected = row["username"].ToString();

                    

                    // Set the background color explicitly after showing MessageBox
                    DataGridRow selectedRow = (DataGridRow)DataGridUsers.ItemContainerGenerator.ContainerFromItem(DataGridUsers.SelectedItem);
                    if (selectedRow != null)
                    {
                        selectedRow.Background = Brushes.Blue;
                        selectedRow.Foreground = Brushes.White;
                    }
                }
                else
                {
                    userDelete.IsEnabled = false;
                    userModify.IsEnabled = false;
                  
                }
            }
        }













        private void userCreate_Click(object sender, RoutedEventArgs e)
        {
            new settingsAddUser().ShowDialog();
            DataGridUsers.ItemsSource = null;
            RefreshDataGridView();
        }

        private void userModify_Click(object sender, RoutedEventArgs e)
        {
           
               

                // Create an instance of settingModifyUsers and pass the username as a parameter
                settingModifyUsers modifyUsersWindow = new settingModifyUsers(UsersDataGridSelected);

                // Show the window
                modifyUsersWindow.ShowDialog();
            
        }



        private void userDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"You are about to delete the user {UsersDataGridSelected}\nAre you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    SqlConnectionConfiguration config = new SqlConnectionConfiguration();
                    
                    String connectionString = config.GetConnectionString();
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "DELETE FROM Users WHERE username = @username";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@username", UsersDataGridSelected);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {

                                MessageBox.Show("User deleted successfully.");
                              
                                RefreshDataGridView();

                            }
                            else
                            {
                                MessageBox.Show("Error deleting the user.");
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message.ToString());
                }
                finally
                {
                    Dispatcher.BeginInvoke((Action)(() => SettingsTabControl.SelectedIndex = SettingsTabControl.SelectedIndex));
                }
            }
        }

        internal void RefreshDataGridView()
        {
            GetFromUsers();
        }
        private void GetFromUsers()
        {
            SqlConnection con = null;
            try
            {

                String query = "SELECT username as 'Username', name as 'Name', email as 'e-mail' FROM Users";

                // Get SQL Server connection details
                SqlConnectionConfiguration config = new SqlConnectionConfiguration();
                string connectionString = config.GetConnectionString();

                // Create and open a connection to the SQL Server
                con = new SqlConnection(connectionString);
                con.Open();

                // Use SqlDataAdapter to fill a DataTable with the query results
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable table = new DataTable();
                adapter.Fill(table);

                // Populate the DataGrid with the DataTable
                DataGridUsers.ItemsSource =null;
                DataGridUsers.ItemsSource = table.DefaultView;
                DataGridUsers.AutoGenerateColumns = true;
                DataGridUsers.CanUserAddRows = false;

            }
            catch (Exception exc)
            {
                // Handle any exceptions and print the error message
                Console.WriteLine(exc.Message.ToString());
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed regardless of exceptions
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}