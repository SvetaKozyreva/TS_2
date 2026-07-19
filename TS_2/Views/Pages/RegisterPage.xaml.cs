using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
using TS_2.Models;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

            if (role == "Тренер")
            {
                if (SecretPasswordBox.Password != "TS_TRAINER_2026")
                {
                    MessageBox.Show("Невірний секретний код тренера.");
                    return;
                }
            }

            if (role == "Адміністратор")
            {
                if (SecretPasswordBox.Password != "TS_ADMIN_2026")
                {
                    MessageBox.Show("Невірний секретний код адміністратора.");
                    return;
                }
            }
            using (AppDbContext db = new AppDbContext())
            {
                if (db.Users.Any(u => u.Login == LoginBox.Text))
                {
                    MessageBox.Show("Користувач з таким логіном вже існує.");

                    return;
                }

                User user = new User();

                user.FullName = NameBox.Text;
                user.Phone = PhoneBox.Text;
                user.Login = LoginBox.Text;
                user.Password = PasswordBox.Password;
                user.Role = role;

                db.Users.Add(user);

                int result = db.SaveChanges();

                MessageBox.Show($"Збережено записів: {result}");

                MessageBox.Show("Акаунт успішно створено!");

                NavigationService.Navigate(new LoginPage());
            }

        }
        private void RoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SecretPanel == null || SecretText == null || RoleBox.SelectedItem == null)
                return;

            string role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

            if (role == "Клієнт")
            {
                SecretPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SecretPanel.Visibility = Visibility.Visible;

                if (role == "Тренер")
                    SecretText.Text = "Секретний код тренера";

                if (role == "Адміністратор")
                    SecretText.Text = "Секретний код адміністратора";
            }
        }
    }
}
