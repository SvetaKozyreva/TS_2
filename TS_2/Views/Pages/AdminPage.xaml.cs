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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private void Users_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminUsersPage());
        }

        private void Trainings_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminTrainingsPage());
        }

        private void Events_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminEventsPage());
        }

        private void Abonements_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminAbonementsPage());
        }
    }
}
