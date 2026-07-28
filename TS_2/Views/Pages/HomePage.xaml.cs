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
                        CornerRadius = new CornerRadius(14),
                        Padding = new Thickness(18),
                        Margin = new Thickness(0, 0, 0, 12),
                        Cursor = Cursors.Hand,
                        BorderBrush = new SolidColorBrush(Color.FromRgb(245, 225, 225)),
                        BorderThickness = new Thickness(1)
                    };

                    border.Effect = new DropShadowEffect
                    {
                        BlurRadius = 10,
                        ShadowDepth = 2,
                        Opacity = 0.18
                    };

                    border.MouseEnter += (s, e) =>
                    {
                        border.Background =
                            new SolidColorBrush(Color.FromRgb(255, 248, 248));
                    };

                    border.MouseLeave += (s, e) =>
                    {
                        border.Background = Brushes.White;
                    };

                    border.MouseLeftButtonUp += (s, e) =>
                    {
                        MainWindow window =
                            (MainWindow)Application.Current.MainWindow;

                        window.Navigate(new SchedulePage(), "Розклад");
                    };



                    Grid grid = new Grid();

                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(220)
                    });

                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });



                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());



                    TextBlock name = new TextBlock
                    {
                        Text = training.Name,
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
                    };

                    Grid.SetRow(name, 0);
                    Grid.SetColumn(name, 0);



                    TextBlock trainerText = new TextBlock
                    {
                        Text = "👤 " + trainer.FullName,
                        Margin = new Thickness(0, 10, 0, 0),
                        FontSize = 14
                    };

                    Grid.SetRow(trainerText, 1);
                    Grid.SetColumn(trainerText, 0);



                    TextBlock date = new TextBlock
                    {
                        Text = "📅 " + training.Date,
                        Margin = new Thickness(0, 6, 0, 0),
                        FontSize = 14
                    };

                    Grid.SetRow(date, 2);
                    Grid.SetColumn(date, 0);



                    TextBlock description = new TextBlock
                    {
                        Text = string.IsNullOrWhiteSpace(training.Description)
                            ? "Опис відсутній."
                            : training.Description.Length > 120
                                ? training.Description.Substring(0, 120) + "..."
                                : training.Description,

                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(20, 0, 20, 0),
                        Foreground = Brushes.Gray,
                        FontSize = 12
                    };

                    Grid.SetColumn(description, 1);
                    Grid.SetRow(description, 0);
                    Grid.SetRowSpan(description, 3);



                    TextBlock time = new TextBlock
                    {
                        Text = "🕒 " + training.Time,
                        FontWeight = FontWeights.Bold,
                        FontSize = 15,
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    Grid.SetColumn(time, 2);
                    Grid.SetRow(time, 0);



                    grid.Children.Add(name);
                    grid.Children.Add(trainerText);
                    grid.Children.Add(date);
                    grid.Children.Add(description);
                    grid.Children.Add(time);

                    border.Child = grid;

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

            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = GridLength.Auto
            });

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            TextBlock title = new TextBlock
            {
                Text = item.Title,
                FontSize = 17,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
            };

            Grid.SetColumn(title, 0);

            TextBlock price = new TextBlock
            {
                Text = item.Price == 0
                    ? "Безкоштовно"
                    : item.Price + " грн",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Grid.SetColumn(price, 1);

            TextBlock date = new TextBlock
            {
                Text = "📅 " + item.Date,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Grid.SetRow(date, 1);
            Grid.SetColumnSpan(date, 2);

            grid.Children.Add(title);
            grid.Children.Add(price);
            grid.Children.Add(date);

            if (!string.IsNullOrWhiteSpace(item.Description))
            {
                grid.RowDefinitions.Add(new RowDefinition());

                TextBlock desc = new TextBlock
                {
                    Text = item.Description.Length > 80
                        ? item.Description.Substring(0, 80) + "..."
                        : item.Description,
                    Margin = new Thickness(0, 12, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Gray
                };

                Grid.SetRow(desc, 2);
                Grid.SetColumnSpan(desc, 2);

                grid.Children.Add(desc);
            }

            border.Child = grid;

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
