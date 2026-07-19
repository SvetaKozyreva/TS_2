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
using TS_2.Models;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }
        private void RegisterTraining_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser == null)
            {
                MessageBox.Show("Спочатку увійдіть у свій акаунт.");
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                TrainingRegistration registration = new TrainingRegistration()
                {
                    UserID = Session.CurrentUser.UserID,
                    TrainingID = 1, // пока первая тренировка
                    RegistrationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                    Status = "Активна"
                };

                db.TrainingRegistrations.Add(registration);

                db.SaveChanges();
            }

            MessageBox.Show("Ви успішно записалися!");
        }
    }

}
