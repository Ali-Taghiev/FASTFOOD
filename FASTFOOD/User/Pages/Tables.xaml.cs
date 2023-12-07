using FASTFOOD.Models;
using System.Collections.Generic;
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
        }
    }
}
