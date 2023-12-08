using System.Windows;
using System.Windows.Controls;

namespace FASTFOOD.User.Components
{
    public partial class Table_Button : UserControl
    {
        public static readonly DependencyProperty TableNumberProperty =
            DependencyProperty.Register("TableNumber", typeof(int), typeof(Table_Button));

        public static readonly DependencyProperty ServiceProperty =
            DependencyProperty.Register("Service", typeof(int), typeof(Table_Button));

        public int TableNumber
        {
            get { return (int)GetValue(TableNumberProperty); }
            set { SetValue(TableNumberProperty, value); }
        }

        public int Service
        {
            get { return (int)GetValue(ServiceProperty); }
            set { SetValue(ServiceProperty, value); }
        }

        public Table_Button()
        {
            InitializeComponent();
        }
    }
}
