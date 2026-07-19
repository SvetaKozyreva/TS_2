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
using Microsoft.EntityFrameworkCore;
using TS_2.Database;
using TS_2.Helpers;
using TS_2.Models;
using TS_2.Views;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            using (var db = new AppDbContext())
            {
                User user = db.Users.FirstOrDefault(u =>
                    u.Login == login &&
                    u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Невірний логін або пароль.");
                    return;
                }

                Session.CurrentUser = user;

                MessageBox.Show($"Ласкаво просимо, {user.FullName}!");

                ((MainWindow)Application.Current.MainWindow).UpdateUser();

                NavigationService.GoBack();
            }
        }
        

    }
}
