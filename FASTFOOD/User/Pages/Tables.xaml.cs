using FASTFOOD.Models;
using FASTFOOD.User.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
       

    }
}
