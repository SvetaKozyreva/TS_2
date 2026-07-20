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
    /// Interaction logic for AddTrainingPage.xaml
    /// </summary>
    
    public partial class AddTrainingPage : Page
    {
        public AddTrainingPage()
        {
            InitializeComponent();
            LoadTrainers();
        }

        private Training editingTraining;
        private void LoadTrainers()
        {
            using (AppDbContext db = new AppDbContext())
            {
                TrainerBox.ItemsSource =
                    db.Users
                    .Where(x => x.Role == "Тренер")
                    .ToList();

                TrainerBox.DisplayMemberPath = "FullName";

                TrainerBox.SelectedValuePath = "UserID";
            }
        }
        public AddTrainingPage(Training training)
        {
            InitializeComponent();

            LoadTrainers();

            editingTraining = training;

            NameBox.Text = training.Name;

            DatePicker.SelectedDate =
                DateTime.Parse(training.Date);

            TimeBox.Text = training.Time;

            PlacesBox.Text =
                training.MaxPlaces.ToString();

            DescriptionBox.Text =
                training.Description;

            TrainerBox.SelectedValue =
                training.TrainerID;
            TitleText.Content = "✏ Редагувати";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                DatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(TimeBox.Text) ||
                string.IsNullOrWhiteSpace(PlacesBox.Text) ||
                TrainerBox.SelectedItem == null)
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            if (!int.TryParse(PlacesBox.Text, out int places))
            {
                MessageBox.Show("Максимальна кількість місць повинна бути числом.");
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                // Якщо редагування
                if (editingTraining != null)
                {
                    Training training = db.Trainings
                        .FirstOrDefault(t => t.TrainingID == editingTraining.TrainingID);

                    training.Name = NameBox.Text;
                    training.Date = DatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");
                    training.Time = TimeBox.Text;
                    training.MaxPlaces = places;
                    training.Description = DescriptionBox.Text;
                    training.TrainerID = (int)TrainerBox.SelectedValue;

                    db.SaveChanges();

                    MessageBox.Show("Тренування успішно оновлено!");
                }
                else
                {
                    Training training = new Training();

                    training.Name = NameBox.Text;
                    training.Date = DatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");
                    training.Time = TimeBox.Text;
                    training.MaxPlaces = places;
                    training.Description = DescriptionBox.Text;
                    training.TrainerID = (int)TrainerBox.SelectedValue;

                    db.Trainings.Add(training);

                    db.SaveChanges();

                    MessageBox.Show("Тренування успішно додано!");
                    NavigationService.Navigate(new AdminTrainingsPage());
                }
            }

            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }

}
