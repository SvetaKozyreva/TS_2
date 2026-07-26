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
    /// Interaction logic for AdminUserDetailsPage.xaml
    /// </summary>
    public partial class AdminUserDetailsPage : Page
    {
        private User currentUser;

        public AdminUserDetailsPage(User user)
        {
            InitializeComponent();

            currentUser = user;

            LoadData();
        }

        private void LoadData()
        {
            NameText.Text = currentUser.FullName;
            LoginText.Text = "Логін: " + currentUser.Login;
            PhoneText.Text = "Телефон: " + currentUser.Phone;
            RoleText.Text = "Роль: " + currentUser.Role;

            using (AppDbContext db = new AppDbContext())
            {
                // ======== Абонемент ========

                UserAbonement userAb = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == currentUser.UserID);

                if (userAb != null)
                {
                    Abonement abonement = db.Abonement
                        .FirstOrDefault(x => x.AbonementID == userAb.AbonementID);

                    AbonementNameText.Text =
                        "Назва: " + abonement.Name;

                    RemainingVisitsText.Text =
                        $"Залишилось занять: {userAb.RemainingVisits}/{abonement.Visits}";

                    EndDateText.Text =
                        "Діє до: " + userAb.EndDate;
                }
                else
                {
                    AbonementNameText.Text =
                        "Активного абонемента немає";

                    RemainingVisitsText.Text = "";

                    EndDateText.Text = "";
                }

                // ======== Записи на тренування ========

                TrainingPanel.Children.Clear();

                var registrations = db.TrainingRegistrations
                    .Where(x => x.UserID == currentUser.UserID)
                    .ToList();

                foreach (var reg in registrations)
                {
                    var training = db.Trainings
                        .FirstOrDefault(x => x.TrainingID == reg.TrainingID);

                    if (training == null)
                        continue;

                    Button btn = new Button
                    {
                        Content = $"{training.Date}   {training.Time}\n{training.Name}",
                        Tag = training,
                        Margin = new Thickness(0, 5, 0, 5),
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Style = (Style)FindResource("MenuButtonStyle")
                    };

                    btn.Click += Training_Click;

                    TrainingPanel.Children.Add(btn);
                }
            }
        }
        private void GiveAbonementButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь позже будет окно выбора абонемента
        }
        private void Training_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Training training = button.Tag as Training;

            ((MainWindow)Application.Current.MainWindow)
                .Navigate(
                    new AdminTrainingDetailsPage(training),
                    "Тренування");
        }
    }
}

