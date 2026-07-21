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
    /// Interaction logic for AddAbonementPage.xaml
    /// </summary>
    public partial class AddAbonementPage : Page
    {
        private Abonement editingAbonement;

        public AddAbonementPage()
        {
            InitializeComponent();
        }

        public AddAbonementPage(Abonement abonement)
        {
            InitializeComponent();

            editingAbonement = abonement;

            NameBox.Text = abonement.Name;
            PriceBox.Text = abonement.Price.ToString();
            VisitsBox.Text = abonement.Visits.ToString();
            DurationBox.Text = abonement.DurationDays.ToString();
            DescriptionBox.Text = abonement.Description;

            SaveButton.Content = "✏ Редагувати";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(PriceBox.Text) ||
                string.IsNullOrWhiteSpace(VisitsBox.Text) ||
                string.IsNullOrWhiteSpace(DurationBox.Text))
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            if (!double.TryParse(PriceBox.Text, out double price))
            {
                MessageBox.Show("Вартість повинна бути числом.");
                return;
            }

            if (!int.TryParse(VisitsBox.Text, out int visits))
            {
                MessageBox.Show("Кількість занять повинна бути числом.");
                return;
            }

            if (!int.TryParse(DurationBox.Text, out int duration))
            {
                MessageBox.Show("Тривалість повинна бути числом.");
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                if (editingAbonement != null)
                {
                    Abonement abonement = db.Abonement
                        .FirstOrDefault(a => a.AbonementID == editingAbonement.AbonementID);

                    abonement.Name = NameBox.Text;
                    abonement.Price = price;
                    abonement.Visits = visits;
                    abonement.DurationDays = duration;
                    abonement.Description = DescriptionBox.Text;

                    db.SaveChanges();

                    MessageBox.Show("Абонемент успішно оновлено!");
                }
                else
                {
                    Abonement abonement = new Abonement
                    {
                        Name = NameBox.Text,
                        Price = price,
                        Visits = visits,
                        DurationDays = duration,
                        Description = DescriptionBox.Text
                    };

                    db.Abonement.Add(abonement);
                    db.SaveChanges();

                    MessageBox.Show("Абонемент успішно додано!");
                }
            }

            NavigationService.Navigate(new AdminAbonementsPage());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminAbonementsPage());
        }
    }
}