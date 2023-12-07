using FASTFOOD.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FASTFOOD.User.Pages
{
    public partial class Tables : UserControl
    {
        public Tables()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            SqlConnectionConfiguration config = new SqlConnectionConfiguration();
            List<Table> tables = config.GetTables();
            List<Service> services = config.GetServices();

            // Clear the existing items
            numberButtonItems.Items.Clear();

            // Set the new ItemsSource
            numberButtonItems.ItemsSource = tables;

            // Do similar binding for services if needed
            // e.g., DataGridServices.ItemsSource = services;
        }


    }
}
