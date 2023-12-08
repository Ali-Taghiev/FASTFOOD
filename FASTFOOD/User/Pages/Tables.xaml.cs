using FASTFOOD.Models;
using FASTFOOD.User.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FASTFOOD.User.Pages
{
    public partial class Tables : UserControl
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
            set { SetValue(SelectedTableProperty, value); }
        }

        public static DependencyProperty SelectedTableProperty =
           DependencyProperty.Register("SelectedTable", typeof(String), typeof(Tables));
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


    }
}
