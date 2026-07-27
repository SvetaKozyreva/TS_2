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
    /// Interaction logic for AdminTrainingDetailsPage.xaml
    /// </summary>
    public partial class AdminTrainingDetailsPage : Page
    {

        private Training currentTraining;

        public AdminTrainingDetailsPage(Training training)
        {
            InitializeComponent();

            currentTraining = training;

            LoadData();
        }
        private void LoadData()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var trainer = db.Users
                    .FirstOrDefault(x => x.UserID == currentTraining.TrainerID);

                TrainingNameText.Text = currentTraining.Name;

                DateText.Text = "📅 Дата: " + currentTraining.Date;

                TimeText.Text = "🕒 Час: " + currentTraining.Time;

                TrainerText.Text = "👤 Тренер: " + trainer?.FullName;

                DescriptionText.Text = currentTraining.Description;

                int busyPlaces = db.TrainingRegistrations
                    .Count(x => x.TrainingID == currentTraining.TrainingID);

                PlacesText.Text =
                    $"👥 Записано: {busyPlaces}/{currentTraining.MaxPlaces}" +
                    $"     Вільно: {currentTraining.MaxPlaces - busyPlaces}";


                var registrations = db.TrainingRegistrations
                    .Where(x => x.TrainingID == currentTraining.TrainingID)
                    .ToList();

                List<User> users = new List<User>();

                foreach (var reg in registrations)
                {
                    User user = db.Users
                        .FirstOrDefault(x => x.UserID == reg.UserID);

                    if (user != null)
                        users.Add(user);
                }

                UsersGrid.ItemsSource = users;
            }
        }
        private void ParticipantsGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
                return;

            User user = UsersGrid.SelectedItem as User;

            ((MainWindow)Application.Current.MainWindow)
                .Navigate(new AdminUserDetailsPage(user), "Користувач");
        }
        private void RegisterClient_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)
                .Navigate(
                    new AdminUsersPage(currentTraining),
                    "Вибір клієнта");
        }
        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)((Button)sender).Tag;

            MessageBoxResult result = MessageBox.Show(
                $"Скасувати запис для {user.FullName}?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (AppDbContext db = new AppDbContext())
            {
                var registration = db.TrainingRegistrations
                    .FirstOrDefault(x =>
                        x.UserID == user.UserID &&
                        x.TrainingID == currentTraining.TrainingID);

                if (registration != null)
                {
                    db.TrainingRegistrations.Remove(registration);
                }

                // Возвращаем занятие по абонементу
                var userAb = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == user.UserID);

                if (userAb != null)
                {
                    var abonement = db.Abonement
                        .FirstOrDefault(x => x.AbonementID == userAb.AbonementID);

                    if (abonement != null &&
                        userAb.RemainingVisits < abonement.Visits)
                    {
                        userAb.RemainingVisits++;
                    }
                }

                db.SaveChanges();
            }

            MessageBox.Show("Запис скасовано.");

            LoadData();
        }
    }
}
