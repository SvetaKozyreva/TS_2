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
    /// Interaction logic for AdminAbonementsPage.xaml
    /// </summary>
    public partial class AdminAbonementsPage : Page
    {
        public AdminAbonementsPage()
        {
            InitializeComponent();
            LoadAbonements();
        }

        private void LoadAbonements()
        {
            using (AppDbContext db = new AppDbContext())
            {
                AbonementsGrid.ItemsSource =
                    db.Abonement.ToList();
            }
        }

        private void AddAbonement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAbonementPage());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Abonement abonement = (Abonement)button.Tag;

            NavigationService.Navigate(new AddAbonementPage(abonement));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Abonement abonement = (Abonement)button.Tag;

            MessageBoxResult result = MessageBox.Show(
                $"Видалити абонемент \"{abonement.Name}\"?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (AppDbContext db = new AppDbContext())
            {
                Abonement a = db.Abonement
                    .FirstOrDefault(x => x.AbonementID == abonement.AbonementID);

                if (a != null)
                {
                    db.Abonement.Remove(a);
                    db.SaveChanges();
                }
            }

            LoadAbonements();
        }

    }
}