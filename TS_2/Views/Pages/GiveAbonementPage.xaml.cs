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
    /// Interaction logic for GiveAbonementPage.xaml
    /// </summary>
    public partial class GiveAbonementPage : Page
    {
        private User currentUser;

        public GiveAbonementPage(User user)
        {
            InitializeComponent();

            currentUser = user;

            UserNameText.Text = user.FullName;

            StartDatePicker.SelectedDate = DateTime.Today;

            LoadAbonements();
        }
        private void LoadAbonements()
        {
            using (AppDbContext db = new AppDbContext())
            {
                AbonementBox.ItemsSource =
                    db.Abonement.ToList();

                AbonementBox.DisplayMemberPath = "Name";

                AbonementBox.SelectedIndex = 0;
            }
        }
        private void Give_Click(object sender, RoutedEventArgs e)
        {
            if (AbonementBox.SelectedItem == null)
                return;

            using (AppDbContext db = new AppDbContext())
            {
                Abonement abonement =
                    (Abonement)AbonementBox.SelectedItem;

                UserAbonement old =
                    db.UserAbonement
                    .FirstOrDefault(x =>
                        x.UserID == currentUser.UserID);

                if (old != null)
                {
                    db.UserAbonement.Remove(old);
                }

                DateTime start =
                    StartDatePicker.SelectedDate.Value;

                UserAbonement userAb =
                    new UserAbonement
                    {
                        UserID = currentUser.UserID,
                        AbonementID = abonement.AbonementID,
                        StartDate = start.ToString("dd.MM.yyyy"),
                        EndDate = start
                            .AddDays(abonement.DurationDays)
                            .ToString("dd.MM.yyyy"),
                        RemainingVisits = abonement.Visits
                    };

                db.UserAbonement.Add(userAb);

                db.SaveChanges();
            }

            MessageBox.Show("Абонемент успішно видано!");

            ((MainWindow)Application.Current.MainWindow)
                .Navigate(
                    new AdminUserDetailsPage(currentUser),
                    "Користувач");
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
