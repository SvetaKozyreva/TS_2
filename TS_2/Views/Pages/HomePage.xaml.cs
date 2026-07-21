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
using TS_2.Helpers;
using TS_2.Models;

namespace TS_2.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            UpdateBanner();

            LoadNearestTrainings();
            LoadNearestEvents();
        }
        private void UpdateBanner()
        {
            if (Session.CurrentUser == null)
            {
                WelcomeTitle.Text = "Ласкаво просимо до TS Studio!";

                WelcomeText.Text =
                    "Почніть шлях до здорового тіла вже сьогодні.";

                WelcomeButton.Content = "Увійти";
            }
            else
            {
                WelcomeTitle.Text =
                    $"Вітаємо, {Session.CurrentUser.FullName}! ❤️";

                WelcomeText.Text =
                    "Бажаємо гарного тренування!";

                WelcomeButton.Content =
                    "Перейти до розкладу";
            }
        }
        private void WelcomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window =
                (MainWindow)Application.Current.MainWindow;

            if (Session.CurrentUser == null)
            {
                window.Navigate(new LoginPage(), "Вхід");
            }
            else
            {
                window.Navigate(new SchedulePage(), "Розклад");
            }
        }
        private void LoadNearestTrainings()
        {
            NearestTrainingsPanel.Children.Clear();

            if (Session.CurrentUser == null)
            {
                TextBlock text = new TextBlock
                {
                    Text = "Увійдіть у свій акаунт,\nщоб переглядати найближчі тренування.",
                    TextWrapping = TextWrapping.Wrap
                };

                text.Style = (Style)FindResource("CardTextStyle");

                NearestTrainingsPanel.Children.Add(text);

                OpenScheduleButton.Content = "Увійти";

                return;
            }

            OpenScheduleButton.Content = "Переглянути всі";

            using (AppDbContext db = new AppDbContext())
            {
                List<Training> trainings = db.Trainings.ToList();

                trainings = trainings
                    .Where(x => DateTime.Parse(x.Date) >= DateTime.Today)
                    .OrderBy(x => DateTime.Parse(x.Date))
                    .Take(3)
                    .ToList();

                if (trainings.Count == 0)
                {
                    NearestTrainingsPanel.Children.Add(
                        new TextBlock
                        {
                            Text = "Наразі тренувань немає.",
                            FontSize = 15
                        });

                    return;
                }

                foreach (Training training in trainings)
                {
                    User trainer = db.Users.FirstOrDefault(x => x.UserID == training.TrainerID);

                    Border border = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(12),
                        Padding = new Thickness(15),
                        Margin = new Thickness(0, 0, 0, 10),
                        Cursor = Cursors.Hand
                    };
                    border.Effect = new DropShadowEffect
                    {
                        BlurRadius = 10,
                        ShadowDepth = 2,
                        Opacity = 0.2
                    };
                    border.MouseEnter += (s, e) =>
                    {
                        border.Background =
                            new SolidColorBrush(Color.FromRgb(255, 245, 245));
                    };

                    border.MouseLeave += (s, e) =>
                    {
                        border.Background = Brushes.White;
                    };
                    border.MouseLeftButtonUp += (s, e) =>
                    {
                        MainWindow window =
                            (MainWindow)Application.Current.MainWindow;

                        window.Navigate(
                            new SchedulePage(),
                            "Розклад");
                    };

                    StackPanel panel = new StackPanel();

                    panel.Children.Add(new TextBlock
                    {
                        Text = training.Date + "   " + training.Time,
                        FontWeight = FontWeights.Bold
                    });

                    panel.Children.Add(new TextBlock
                    {
                        Text = training.Name
                    });

                    panel.Children.Add(new TextBlock
                    {
                        Text = "Тренер: " + trainer.FullName
                    });

                    border.Child = panel;

                    NearestTrainingsPanel.Children.Add(border);
                }
            }
        }
        private void LoadNearestEvents()
        {
            NearestEventsPanel.Children.Clear();

            using (AppDbContext db = new AppDbContext())
            {
                List<Event> events = db.Event.ToList();

                events = events
                    .Where(x => DateTime.Parse(x.Date) >= DateTime.Today)
                    .OrderBy(x => DateTime.Parse(x.Date))
                    .Take(3)
                    .ToList();

                if (events.Count == 0)
                {
                    TextBlock text = new TextBlock
                    {
                        Text = "Наразі найближчих подій немає."
                    };

                    text.Style = (Style)FindResource("CardTextStyle");

                    NearestEventsPanel.Children.Add(text);

                    return;
                }

                foreach (Event item in events)
                {
                    CreateEventCard(item);
                }
            }
        }
        private void CreateEventCard(Event item)
        {
            Border border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(12),
                BorderBrush = new SolidColorBrush(Color.FromRgb(240, 220, 220)),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(12),
                Cursor = Cursors.Hand
            };

            StackPanel panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = "🎉 " + item.Title,
                FontWeight = FontWeights.Bold,
                FontSize = 15
            });

            panel.Children.Add(new TextBlock
            {
                Text = "📅 " + item.Date,
                Margin = new Thickness(0, 4, 0, 0)
            });

            panel.Children.Add(new TextBlock
            {
                Text = "💰 " + item.Price + " грн",
                Margin = new Thickness(0, 3, 0, 0)
            });

            border.Child = panel;

            border.MouseEnter += (s, e) =>
            {
                border.Background =
                    new SolidColorBrush(Color.FromRgb(255, 245, 245));
            };

            border.MouseLeave += (s, e) =>
            {
                border.Background = Brushes.White;
            };

            border.MouseLeftButtonUp += (s, e) =>
            {
                MainWindow window =
                    (MainWindow)Application.Current.MainWindow;

                window.Navigate(new EventsPage(), "Події");
            };

            NearestEventsPanel.Children.Add(border);
        }
        private void OpenEventsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window =
                (MainWindow)Application.Current.MainWindow;

            window.Navigate(new EventsPage(), "Події");
        }

        private void OpenScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window =
                (MainWindow)Application.Current.MainWindow;

            if (Session.CurrentUser == null)
            {
                window.Navigate(new LoginPage(), "Вхід");
            }
            else
            {
                window.Navigate(new SchedulePage(), "Розклад");
            }
        }

    }
}
