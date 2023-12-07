// In Table_Button.xaml.cs

using System.Windows;
using System.Windows.Controls;

namespace FASTFOOD.User.Components
{
    public partial class Table_Button : UserControl
    {
        public Table_Button()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Using a DependencyProperty as the backing store for TableNumber. This enables binding.
        public static readonly DependencyProperty TableNumberProperty =
            DependencyProperty.Register("TableNumber", typeof(string), typeof(Table_Button));

        public string TableNumber
        {
            get { return (string)GetValue(TableNumberProperty); }
            set { SetValue(TableNumberProperty, value); }
        }

        // Loaded event handler for Table_Button
        private void Table_Button_Loaded(object sender, RoutedEventArgs e)
        {
            // Your logic when the Table_Button is loaded goes here
            // For example, you can display the TableNumber in a MessageBox
            MessageBox.Show($"Table {TableNumber} loaded.");
        }

        // ... (your other properties and methods)
    }
}
