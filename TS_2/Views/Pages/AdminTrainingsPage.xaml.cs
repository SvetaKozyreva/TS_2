using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TS_2.Database;

namespace TS_2.Views.Pages
{
    public partial class AdminTrainingsPage : Page
    {
        public AdminTrainingsPage()
        {
            InitializeComponent();

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

        }
    }
}