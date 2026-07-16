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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TS_2.Views.Pages;

namespace TS_2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new HomePage());
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SchedulePage());
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EventsPage());
        }

        private void AbonementsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AbonementsPage());
        }
    }

}
