using FASTFOOD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FASTFOOD.User
{
    

    public partial class DashBoard : Window
    {
        private bool Expanded = false;
       

        public DashBoard()
        {
            InitializeComponent();
            contentContainer.Content = new User.Pages.Tables();
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            GridLength OpenSize = new GridLength(180, GridUnitType.Pixel);
            sidebar.Width = OpenSize;
            int from, to;
            if (Expanded)
            {
                from = 180;
                to = 60;
                Expanded = false;
            }
            else
            {
                from = 60;
                to = 180;
                Expanded = true;
            }
            Storyboard storyboard = new Storyboard();

            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            CubicEase ease = new CubicEase { EasingMode = EasingMode.EaseOut };
            DoubleAnimation animation = new DoubleAnimation();
            animation.EasingFunction = ease;
            animation.Duration = duration;
            storyboard.Children.Add(animation);
            animation.From = from;
            animation.To = to;
            Storyboard.SetTarget(animation, sidebar);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

            storyboard.Begin();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            
            App.Current.MainWindow.Show();
            this.Close();
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            settings form = new settings();
            form.ShowDialog();
        }
        private void btnTables_Click(object sender, RoutedEventArgs e)
        {
            contentContainer.Content = new User.Pages.Tables();
        }

    }
}
