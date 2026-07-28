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
using Microsoft.EntityFrameworkCore;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for MyTrainingsPage.xaml
    /// </summary>
    public partial class MyTrainingsPage : Page
    {
        public MyTrainingsPage()
        {
            InitializeComponent();
            LoadTrainings();
        }
        private void LoadTrainings()
        {
            TrainingsPanel.Children.Clear();

            using (AppDbContext db = new AppDbContext())
            {
                var registrations = db.TrainingRegistrations
                    .Where(r => r.UserID == Session.CurrentUser.UserID)
                    .ToList();

                foreach (var registration in registrations)
                {
                    var training = db.Trainings
                        .FirstOrDefault(t => t.TrainingID == registration.TrainingID);

                    if (training == null)
                        continue;

                    Border card = new Border()
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(15),
                        Padding = new Thickness(20),
                        Margin = new Thickness(0, 0, 0, 15)
                    };

                    StackPanel stack = new StackPanel();

                    stack.Children.Add(new TextBlock()
                    {
                        Text = training.Name,
                        FontSize = 22,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.DarkSlateGray
                    });

                    stack.Children.Add(new TextBlock()
                    {
                        Text = "📅 " + training.Date,
                        Margin = new Thickness(0, 10, 0, 0)
                    });

                    stack.Children.Add(new TextBlock()
                    {
                        Text = "🕒 " + training.Time
                    });

                    stack.Children.Add(new TextBlock()
                    {
                        Text = "Статус: " + registration.Status
                    });

                    Button cancelButton = new Button()
                    {
                        Content = "Скасувати запис",
                        Width = 160,
                        Height = 38,
                        Margin = new Thickness(0, 20, 0, 0),
                        Style = (Style)FindResource("PrimaryButtonStyle")
                    };

                    cancelButton.Tag = registration;

                    cancelButton.Click += CancelButton_Click;

                    stack.Children.Add(cancelButton);

                    card.Child = stack;

                    TrainingsPanel.Children.Add(card);
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            TrainingRegistration registration = (TrainingRegistration)button.Tag;

            using (AppDbContext db = new AppDbContext())
            {
                var reg = db.TrainingRegistrations
                    .FirstOrDefault(r => r.RegistrationID == registration.RegistrationID);

                if (reg != null)
                {
                    db.TrainingRegistrations.Remove(reg);

                    db.SaveChanges();
                }
            }

            MessageBox.Show("Запис скасовано.");

            LoadTrainings();
        }
    }
}
