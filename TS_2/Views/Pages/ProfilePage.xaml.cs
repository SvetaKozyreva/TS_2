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
using TS_2.Helpers;
using TS_2.Views;
using TS_2.Views.Pages;


namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            if (Session.CurrentUser != null)
            {
                FullNameText.Text = Session.CurrentUser.FullName;
                LoginText.Text = "Логін: " + Session.CurrentUser.Login;
                PhoneText.Text = "Телефон: " + Session.CurrentUser.Phone;
                RoleText.Text = "Роль: " + Session.CurrentUser.Role;
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;

            MainWindow window = (MainWindow)Application.Current.MainWindow;

            window.UpdateUser();

            window.MainFrame.Navigate(new HomePage());
        }
        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функція буде додана пізніше 😊");
        }
        private void MyTrainings_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)
                .Navigate(new MyTrainingsPage(), "Мої записи");
        }

    }
}
