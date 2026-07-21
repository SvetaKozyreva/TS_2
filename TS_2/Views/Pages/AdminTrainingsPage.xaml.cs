using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TS_2.Database;
using TS_2.Models;

namespace TS_2.Views.Pages
{
    public partial class AdminTrainingsPage : Page
    {
        public AdminTrainingsPage()
        {
            InitializeComponent();
            Loaded += AdminTrainingsPage_Loaded;
            LoadTrainings();
        }
        private void AdminTrainingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainings();
        }
        private void LoadTrainings()
        {
            using (AppDbContext db = new AppDbContext())
            {
                TrainingsGrid.ItemsSource = db.Trainings.ToList();
            }
        }

        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTrainingPage());
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Training training = (Training)button.Tag;

            NavigationService.Navigate(new AddTrainingPage(training));
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Training training = (Training)button.Tag;

            MessageBoxResult result = MessageBox.Show(
                $"Видалити тренування \"{training.Name}\"?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (AppDbContext db = new AppDbContext())
            {
                // Удаляем все записи клиентов на тренировку
                var registrations = db.TrainingRegistrations
                    .Where(r => r.TrainingID == training.TrainingID)
                    .ToList();

                db.TrainingRegistrations.RemoveRange(registrations);

                // Удаляем саму тренировку
                var tr = db.Trainings
                    .FirstOrDefault(t => t.TrainingID == training.TrainingID);

                if (tr != null)
                {
                    db.Trainings.Remove(tr);
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Тренування видалено.");

            LoadTrainings();
        }



    }
}