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
        private DateTime selectedDate;
        private DateTime firstDay;


        public SchedulePage()
        {
            InitializeComponent();

            selectedDate = DateTime.Today;
            firstDay = DateTime.Today;

            Loaded += SchedulePage_Loaded;
        }
        private void SchedulePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainers();

            CreateDays();

            LoadTrainings();
        }
        private void LoadTrainers()
        {
            using (AppDbContext db = new AppDbContext())
            {

                TrainerBox.Items.Clear();


                TrainerBox.Items.Add(
                    new ComboBoxItem
                    {
                        Content = "Усі тренери",
                        Tag = null
                    });



                var trainers =
                    db.Users
                    .Where(x => x.Role == "Тренер")
                    .ToList();



                foreach (var trainer in trainers)
                {
                    TrainerBox.Items.Add(
                        new ComboBoxItem
                        {
                            Content = trainer.FullName,
                            Tag = trainer.UserID
                        });
                }


                TrainerBox.SelectedIndex = 0;

            }
        }
        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            LoadTrainings();
        }


        // Создаем дни недели автоматически
        private void CreateDays()
        {
            DaysPanel.Children.Clear();


            for (int i = 0; i < 7; i++)
            {
                DateTime day = firstDay.AddDays(i);


                Button button = new Button();

                button.Style =
                    (Style)FindResource("DayButtonStyle");


                StackPanel panel = new StackPanel();


                TextBlock dayName = new TextBlock
                {
                    Text = day.ToString("ddd").ToUpper(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.SemiBold
                };


                TextBlock date = new TextBlock
                {
                    Text = day.ToString("dd.MM"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 12
                };


                panel.Children.Add(dayName);
                panel.Children.Add(date);


                button.Content = panel;

                button.Tag = day;

                button.Click += Day_Click;


                DaysPanel.Children.Add(button);



                if (day.Date == selectedDate.Date)
                {
                    button.Style =
                    (Style)FindResource("ActiveDayButtonStyle");
                }
            }
        }



        private void Day_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;


            selectedDate = (DateTime)btn.Tag;


            LoadTrainings();



            foreach (Button b in DaysPanel.Children)
            {
                b.Style =
                (Style)FindResource("DayButtonStyle");
            }


            btn.Style =
            (Style)FindResource("ActiveDayButtonStyle");
        }




        private void LoadTrainings()
        {
            if (TrainingsPanel == null)
                return;

            TrainingsPanel.Children.Clear();


            using (AppDbContext db = new AppDbContext())
            {

                string date =
                    selectedDate.ToString("dd.MM.yyyy");


                var trainings =
                    db.Trainings
                    .Where(x => x.Date == date)
                    .ToList();



                // Фильтр направления

                if (DirectionBox.SelectedItem is ComboBoxItem direction)
                {
                    string value = direction.Content.ToString();


                    if (value != "Усі напрямки")
                    {
                        trainings = trainings
                        .Where(x => x.Name == value)
                        .ToList();
                    }
                }



                // Фильтр времени

                if (TimeBox.SelectedItem is ComboBoxItem time)
                {
                    string value = time.Content.ToString();


                    if (value != "Увесь день")
                    {

                        if (value == "Ранок")
                        {
                            trainings = trainings
                             .Where(x =>
                             TimeSpan.Parse(x.Time.Split('-')[0].Trim())
                             < new TimeSpan(12, 0, 0))
                             .ToList();
                        }


                        if (value == "День")
                        {
                            trainings = trainings
                            .Where(x =>
                            TimeSpan.Parse(x.Time.Split('-')[0].Trim())
                            >= new TimeSpan(12, 0, 0)
                            &&
                            TimeSpan.Parse(x.Time.Split('-')[0].Trim())
                            < new TimeSpan(17, 0, 0))
                            .ToList();
                        }


                        if (value == "Вечір")
                        {
                            trainings = trainings
                            .Where(x =>
                            TimeSpan.Parse(x.Time.Split('-')[0].Trim())
                            >= new TimeSpan(17, 0, 0))
                            .ToList();
                        }

                    }

                }



                // Фильтр тренера

                if (TrainerBox.SelectedItem is ComboBoxItem trainerItem
    &&
    trainerItem.Tag != null)
                {
                    int trainerId =
                        (int)trainerItem.Tag;


                    trainings =
                        trainings
                        .Where(x => x.TrainerID == trainerId)
                        .ToList();
                }


                foreach (var training in trainings.ToList())
                {
                    CreateTrainingCard(training);
                }

            }
        }




        private void CreateTrainingCard(Training training)
        {
            Border card = new Border
            {
                Style = (Style)FindResource("CardStyle"),
                Margin = new Thickness(0, 0, 0, 15)
            };

            Grid grid = new Grid();

            // 4 колонки:
            // 1 - название/время/тренер
            // 2 - описание
            // 3 - места
            // 4 - кнопка
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.8, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.3, GridUnitType.Star) });

            using (AppDbContext db = new AppDbContext())
            {
                User trainer = db.Users
                    .FirstOrDefault(x => x.UserID == training.TrainerID);

                int busyPlaces = db.TrainingRegistrations
                    .Count(x => x.TrainingID == training.TrainingID);

                int freePlaces = training.MaxPlaces - busyPlaces;

                //--------------------------------------------------
                // Левая часть
                //--------------------------------------------------

                StackPanel left = new StackPanel();

                StackPanel top = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                top.Children.Add(new TextBlock
                {
                    Text = training.Name,
                    FontSize = 22,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
                });

                top.Children.Add(new TextBlock
                {
                    Text = "   |   🕒 " + training.Time,
                    Margin = new Thickness(15, 4, 0, 0),
                    FontSize = 15,
                    Foreground = Brushes.Gray
                });

                left.Children.Add(top);

                left.Children.Add(new TextBlock
                {
                    Text = "👤Тренер: " + trainer.FullName,
                    Margin = new Thickness(0, 10, 0, 0),
                    FontSize = 15,
                    Foreground = Brushes.DimGray
                });

                Grid.SetColumn(left, 0);
                grid.Children.Add(left);

                //--------------------------------------------------
                // Описание
                //--------------------------------------------------

                TextBlock description = new TextBlock
                {
                    Text = training.Description,
                    Margin = new Thickness(25, 0, 20, 0),
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    Foreground = Brushes.Gray,
                    MaxHeight = 60
                };

                Grid.SetColumn(description, 1);
                grid.Children.Add(description);

                //--------------------------------------------------
                // Места
                //--------------------------------------------------

                Border placesBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(252, 238, 238)),
                    CornerRadius = new CornerRadius(12),
                    Padding = new Thickness(12, 8, 12, 8),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                placesBorder.Child = new TextBlock
                {
                    Text = $"👥\n{freePlaces}/{training.MaxPlaces}",
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
                };

                Grid.SetColumn(placesBorder, 2);
                grid.Children.Add(placesBorder);

                //--------------------------------------------------
                // Кнопка
                //--------------------------------------------------

                Button btn = new Button
                {
                    Content = "Записатися",
                    Width = 120,
                    Height = 40,
                    Tag = training,
                    Style = (Style)FindResource("PrimaryButtonStyle"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };

                btn.Click += RegisterTraining_Click;

                Grid.SetColumn(btn, 3);
                grid.Children.Add(btn);
            }

            card.Child = grid;

            TrainingsPanel.Children.Add(card);
        }




        private void RegisterTraining_Click(object sender, RoutedEventArgs e)
        {

            if (Session.CurrentUser == null)
            {
                MessageBox.Show(
                "Спочатку увійдіть у свій акаунт.");
                return;
            }


            Button btn = sender as Button;


            Training training =
            btn.Tag as Training;



            using (AppDbContext db = new AppDbContext())
            {
                bool alreadyRegistered =
                    db.TrainingRegistrations.Any(x =>
                        x.UserID == Session.CurrentUser.UserID &&
                        x.TrainingID == training.TrainingID);

                if (alreadyRegistered)
                {
                    MessageBox.Show("Ви вже записані на це тренування.");
                    return;
                }
                // Проверяем, есть ли активный абонемент
                UserAbonement userAbonement = db.UserAbonement
                    .FirstOrDefault(x =>
                        x.UserID == Session.CurrentUser.UserID &&
                        x.RemainingVisits > 0);

                if (userAbonement == null)
                {
                    MessageBox.Show(
                        "У вас немає активного абонемента.\nПридбайте новий абонемент.");
                    return;
                }
                int busyPlaces =
                    db.TrainingRegistrations.Count(x =>
                        x.TrainingID == training.TrainingID);

                if (busyPlaces >= training.MaxPlaces)
                {
                    MessageBox.Show("На це тренування вже немає вільних місць.");
                    return;
                }

                TrainingRegistration reg =
                new TrainingRegistration
                {
                    UserID =
                    Session.CurrentUser.UserID,

                    TrainingID =
                    training.TrainingID,

                    RegistrationDate =
                    DateTime.Now.ToString(),

                    Status = "Активна"
                };


                db.TrainingRegistrations.Add(reg);
                // Списываем одно занятие
                userAbonement.RemainingVisits--;

                if (userAbonement.RemainingVisits == 0)
                {
                    MessageBox.Show(
                        "Це було останнє заняття за вашим абонементом.\nПридбайте новий абонемент.");
                }

                db.SaveChanges();

            }


            MessageBox.Show("Ви успішно записалися!");

            // Обновляем список тренировок
            LoadTrainings();

            CreateDays();
        }
        private void PreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            firstDay = firstDay.AddDays(-7);

            CreateDays();
        }



        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            firstDay = firstDay.AddDays(7);

            CreateDays();
        }


    }

}
