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
using TS_2.Database;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        public AdminUsersPage()
        {
            InitializeComponent();

            LoadUsers();
        }

        private void LoadUsers()
        {
            using (AppDbContext db = new AppDbContext())
            {
                UsersGrid.ItemsSource = db.Users.ToList();
            }
        }
        private void UsersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
                return;

            Models.User user = UsersGrid.SelectedItem as Models.User;

            ((MainWindow)Application.Current.MainWindow)
                .Navigate(new AdminUserDetailsPage(user), "Користувач");
        }
    }
}
