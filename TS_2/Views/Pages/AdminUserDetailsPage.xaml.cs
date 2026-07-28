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
using System.Windows.Media.Effects;
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

                    Border card = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(12),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(240, 220, 220)),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(15),
                        Margin = new Thickness(0, 0, 0, 10),
                        Cursor = Cursors.Hand
                    };

                    card.Effect = new DropShadowEffect
                    {
                        BlurRadius = 10,
                        ShadowDepth = 2,
                        Opacity = 0.18
                    };

                    card.MouseEnter += (s, e) =>
                    {
                        card.Background =
                            new SolidColorBrush(Color.FromRgb(255, 248, 248));
                    };

                    card.MouseLeave += (s, e) =>
                    {
                        card.Background = Brushes.White;
                    };

                    card.MouseLeftButtonDown += (s, e) =>
                    {
                        if (e.ClickCount == 2)
                        {
                            ((MainWindow)Application.Current.MainWindow)
                                .Navigate(
                                    new AdminTrainingDetailsPage(training),
                                    "Тренування");

                            e.Handled = true;
                        }
                    };

                    Grid grid = new Grid();

                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                    StackPanel info = new StackPanel();

                    info.Children.Add(new TextBlock
                    {
                        Text = training.Name,
                        FontSize = 19,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "👤 " + db.Users
                            .FirstOrDefault(x => x.UserID == training.TrainerID)?.FullName,
                        Margin = new Thickness(0, 10, 0, 0),
                        FontSize = 15
                    });

                    info.Children.Add(new TextBlock
                    {
                        Text = "📅 " + training.Date,
                        Margin = new Thickness(0, 6, 0, 0),
                        FontSize = 15
                    });

                    Grid.SetColumn(info, 0);
                    grid.Children.Add(info);

                    TextBlock time = new TextBlock
                    {
                        Text = "🕒 " + training.Time,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    Grid.SetColumn(time, 1);
                    grid.Children.Add(time);

                    card.Child = grid;

                    TrainingPanel.Children.Add(card);
                }
            }
        }
        private void GiveAbonementButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new GiveAbonementPage(currentUser));
        }
        private void EditAbonement_Click(object sender, RoutedEventArgs e)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var userAb = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == currentUser.UserID);

                if (userAb == null)
                {
                    MessageBox.Show("У користувача немає абонемента.");
                    return;
                }

                RemainingVisitsBox.Text =
                    userAb.RemainingVisits.ToString();

                EndDatePicker.SelectedDate =
                    DateTime.Parse(userAb.EndDate);

                EditAbonementPanel.Visibility =
                    Visibility.Visible;
            }
        }
        private void SaveAbonement_Click(object sender, RoutedEventArgs e)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var userAb = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == currentUser.UserID);

                if (userAb == null)
                    return;

                userAb.RemainingVisits =
                    int.Parse(RemainingVisitsBox.Text);

                userAb.EndDate =
                    EndDatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");

                db.SaveChanges();
            }

            EditAbonementPanel.Visibility =
                Visibility.Collapsed;

            LoadData();

            MessageBox.Show("Абонемент оновлено.");
        }
        private void CancelAbonement_Click(object sender, RoutedEventArgs e)
        {
            EditAbonementPanel.Visibility =
                Visibility.Collapsed;
        }
        private void DeleteAbonement_Click(object sender, RoutedEventArgs e)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var userAb = db.UserAbonement
                    .FirstOrDefault(x => x.UserID == currentUser.UserID);

                if (userAb == null)
                {
                    MessageBox.Show("У користувача немає активного абонемента.");
                    return;
                }

                if (MessageBox.Show(
                    "Забрати абонемент?",
                    "Підтвердження",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    db.UserAbonement.Remove(userAb);
                    db.SaveChanges();

                    MessageBox.Show("Абонемент успішно видалено.");

                    LoadData();
                }
            }
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

