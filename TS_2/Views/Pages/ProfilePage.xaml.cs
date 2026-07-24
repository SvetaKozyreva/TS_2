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
                LoadAbonement();
                LoadNearestTraining();
            }
        }
        private void LoadAbonement()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var userAbonement = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == Session.CurrentUser.UserID);

                if (userAbonement == null)
                {
                    AbonementText.Text =
                        "Активний абонемент відсутній.";

                    return;
                }

                var abonement = db.Abonement
                    .FirstOrDefault(x =>
                        x.AbonementID == userAbonement.AbonementID);

                if (abonement == null)
                {
                    AbonementText.Text =
                        "Активний абонемент відсутній.";

                    return;
                }

                AbonementText.Text =
                    abonement.Name +
                    "\n\n💰 " + abonement.Price + " грн" +
                    "\n📅 До: " + userAbonement.EndDate +
                    "\n🏋 Залишилось занять: "
                    + userAbonement.RemainingVisits +
                    " / " + abonement.Visits;
            }
        }
        private void LoadNearestTraining()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var registration = db.TrainingRegistrations
                    .Where(x =>
                        x.UserID == Session.CurrentUser.UserID &&
                        x.Status == "Активна")
                    .ToList()
                    .OrderBy(x => DateTime.Parse(x.RegistrationDate))
                    .FirstOrDefault();

                if (registration == null)
                {
                    TrainingText.Text = "Немає записів";
                    return;
                }

                var training = db.Trainings
                    .FirstOrDefault(x =>
                        x.TrainingID == registration.TrainingID);

                if (training == null)
                {
                    TrainingText.Text = "Немає записів";
                    return;
                }

                var trainer = db.Users
                    .FirstOrDefault(x =>
                        x.UserID == training.TrainerID);

                TrainingText.Text =
                    training.Name +
                    "\n" +
                    training.Date +
                    " " +
                    training.Time +
                    "\nТренер: " +
                    trainer.FullName;
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
