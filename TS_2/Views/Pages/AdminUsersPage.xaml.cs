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
using TS_2.Models;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        private Training currentTraining;
        private bool chooseMode;
        public AdminUsersPage()
        {
            InitializeComponent();

            chooseMode = false;

            LoadUsers();
        }
        public AdminUsersPage(Training training)
        {
            InitializeComponent();

            currentTraining = training;

            chooseMode = true;

            LoadUsers();
        }

        private void LoadUsers()
        {
            using (AppDbContext db = new AppDbContext())
            {
                UsersGrid.ItemsSource = db.Users.ToList();
            }
            RegisterColumn.Visibility =
    chooseMode
    ? Visibility.Visible
    : Visibility.Collapsed;
        }
        private void UsersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
                return;

            Models.User user = UsersGrid.SelectedItem as Models.User;

            ((MainWindow)Application.Current.MainWindow)
                .Navigate(new AdminUserDetailsPage(user), "Користувач");
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)((Button)sender).Tag;

            using (AppDbContext db = new AppDbContext())
            {
                bool alreadyRegistered =
                    db.TrainingRegistrations.Any(x =>
                        x.UserID == user.UserID &&
                        x.TrainingID == currentTraining.TrainingID);

                if (alreadyRegistered)
                {
                    MessageBox.Show("Клієнт вже записаний.");
                    return;
                }

                int busyPlaces =
                    db.TrainingRegistrations.Count(x =>
                        x.TrainingID == currentTraining.TrainingID);

                if (busyPlaces >= currentTraining.MaxPlaces)
                {
                    MessageBox.Show("Вільних місць немає.");
                    return;
                }

                UserAbonement abonement =
                    db.UserAbonement.FirstOrDefault(x =>
                        x.UserID == user.UserID);

                if (abonement == null)
                {
                    MessageBox.Show("У клієнта немає активного абонемента.");
                    return;
                }

                if (abonement.RemainingVisits <= 0)
                {
                    MessageBox.Show("У клієнта закінчилися заняття.");
                    return;
                }

                abonement.RemainingVisits--;

                TrainingRegistration registration =
                    new TrainingRegistration
                    {
                        UserID = user.UserID,
                        TrainingID = currentTraining.TrainingID,
                        RegistrationDate = DateTime.Now.ToString(),
                        Status = "Активна"
                    };

                db.TrainingRegistrations.Add(registration);

                db.SaveChanges();
            }

            MessageBox.Show("Клієнта успішно записано.");

            NavigationService.Navigate(
                new AdminTrainingDetailsPage(currentTraining));
        }
    }
}
