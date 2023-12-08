using FASTFOOD.Models;
using FASTFOOD.User.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FASTFOOD.User.Pages
{
    public partial class Tables : UserControl, INotifyPropertyChanged
    {
        public List<Table> NumberButtonItems { get; set; }

        public Tables()
        {
            InitializeComponent();
            LoadData();
            DataContext = this; // Set the DataContext to the current instance of the Tables class

        }


        private void LoadData()
        {
            SqlConnectionConfiguration config = new SqlConnectionConfiguration();
            NumberButtonItems = config.GetTables();

            // Add this line to check the Id of the first item after loading data
            System.Diagnostics.Debug.WriteLine("First item Id: " + NumberButtonItems.FirstOrDefault()?.Id);
            foreach (var table in NumberButtonItems)
            {
                Table_Button tableButton = new Table_Button();
                tableButton.DataContext = table;
                // Add your Table_Button to the UI
            }
        }
        public String SelectedTable
        {
            get { return (String)GetValue(SelectedTableProperty); }
            set
            {
                SetValue(SelectedTableProperty, value);
                NotifyPropertyChanged("Title");
            }
        }
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public static DependencyProperty SelectedTableProperty =
           DependencyProperty.Register("SelectedTable", typeof(String), typeof(Tables));

        public event PropertyChangedEventHandler PropertyChanged;




        private void Table_Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the Table_Button in the visual tree hierarchy
            var tableButton = FindParent<Components.Table_Button>((DependencyObject)e.OriginalSource);

            if (tableButton != null)
            {
                var tableNumber = tableButton.GetValue(Components.Table_Button.TableNumberProperty);
                MessageBox.Show(tableNumber.ToString());
                SelectedTable = tableNumber.ToString();

                // Mark the event as handled to prevent it from propagating further up the visual tree
                e.Handled = true;
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Traverse up the visual tree to find a parent of type T
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (T)parent;
        }



        private void serviceStart_Click(object sender, RoutedEventArgs e)
        {
            long serviceID = -1;

            String query;
            SqlCommand cmd;
            SqlConnection con = null;

            // Create service
            try
            {
                // Create an instance of SqlConnectionConfiguration
                SqlConnectionConfiguration connectionConfig = new SqlConnectionConfiguration();

                // Establish connection
                using (con = connectionConfig.GetSqlConnection())
                {
                    con.Open();

                    // Insert into services and retrieve the service ID
                    query = "INSERT INTO Services (start) VALUES (CURRENT_TIMESTAMP); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(query, con);

                    // Use ExecuteScalar to get the result of the SELECT SCOPE_IDENTITY()
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        serviceID = Convert.ToInt64(result);
                    }
                    else
                    {
                        MessageBox.Show("Error creating the service.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Exit the method to avoid further processing
                    }
                }

                // Assign to the current table
                if (serviceID != -1 && SelectedTable != null)
                {
                    try
                    {
                        // Create an instance of SqlConnectionConfiguration
                        

                        // Establish connection
                        using (con = connectionConfig.GetSqlConnection())
                        {
                            con.Open();

                            // Update actualServiceID in the tables table
                            query = "UPDATE tables SET actualServiceID = @serviceID WHERE tables.id = @selectedTableID;";
                            cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@serviceID", serviceID);

                            // Check if SelectedTable is not null before calling ToString()
                            if (SelectedTable != null)
                            {
                                cmd.Parameters.AddWithValue("@selectedTableID", SelectedTable.ToString());
                            }
                            else
                            {
                                MessageBox.Show("SelectedTable is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return; // Exit the method to avoid further processing
                            }

                            using (System.Data.IDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    MessageBox.Show("Error assigning the service to this table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else
                                {
                                    // TODO OK
                                    // Reload the view
                                    LoadData();
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("There was an error adding the user: \n" + exc.Message.ToString() + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("There was an error adding the user: \n" + exc.Message.ToString() + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }


        }

        private void serviceFinish_Click(object sender, RoutedEventArgs e)
        {
            Boolean setTime = false;

            String query;
            SqlCommand cmd;
            SqlConnection con = null;

            // Update service
            try
            {
                // Create an instance of SqlConnectionConfiguration
                SqlConnectionConfiguration connectionConfig = new SqlConnectionConfiguration();

                // Establish connection
                using (con = connectionConfig.GetSqlConnection())
                {
                    con.Open();

                    // Update end time in the services table
                    query = "UPDATE Services SET [end] = CURRENT_TIMESTAMP WHERE Services.id = (SELECT actualServiceID FROM tables WHERE id = 1)";
                    cmd = new SqlCommand(query, con);

                    // Use ExecuteNonQuery since no result set is expected
                    cmd.ExecuteNonQuery();

                    setTime = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("There was an error closing the service: \n" + exc.Message.ToString() + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            // Assign to the current table
            if (setTime)
            {
                try
                {
                    // Create an instance of SqlConnectionConfiguration
                    SqlConnectionConfiguration connectionConfig = new SqlConnectionConfiguration();

                    // Establish connection
                    using (con = connectionConfig.GetSqlConnection())
                    {
                        con.Open();
                        
                        // Clear actualServiceID in the tables table
                        query = "UPDATE tables SET actualServiceID = null WHERE tables.id = @selectedTableID;";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@selectedTableID", SelectedTable?.ToString()); 

                        // Use ExecuteNonQuery since no result set is expected
                        cmd.ExecuteNonQuery();

                        // TODO OK
                        // Reload the view
                        LoadData();
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("There was an error closing the order: \n" + exc.Message.ToString() + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
        }




    }
}
