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

namespace FASTFOOD.User
{
  
    public partial class settings : Window
    {
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

        private void userCreate_Click(object sender, RoutedEventArgs e)
        {
            new settingsAddUser().ShowDialog();
            
        }
    }
}