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
    /// Interaction logic for AbonementsPage.xaml
    /// </summary>
    public partial class AbonementsPage : Page
    {
        public AbonementsPage()
        {
            InitializeComponent();
            LoadCards();
        }

        private void LoadCards()
        {
            CardsPanel.Children.Clear();

            using (AppDbContext db = new AppDbContext())
            {
                foreach (Abonement abonement in db.Abonement.ToList())
                {
                    Border card = new Border
                    {
                        Width = 380,
                        Margin = new Thickness(15),
                        Padding = new Thickness(25),
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(18),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(245, 225, 225)),
                        BorderThickness = new Thickness(1)
                    };

                    StackPanel panel = new StackPanel();

                    panel.Children.Add(new TextBlock
                    {
                        Text = "💳 " + abonement.Name,
                        FontSize = 24,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(109, 85, 85))
                    });

                    panel.Children.Add(new TextBlock
                    {
                        Text = $"{abonement.Price} ₴",
                        Margin = new Thickness(0, 15, 0, 10),
                        FontSize = 26,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(200, 109, 109))
                    });

                    panel.Children.Add(new TextBlock
                    {
                        Text = $"📅 {abonement.DurationDays} днів",
                        FontSize = 15,
                        Margin = new Thickness(0, 5, 0, 0)
                    });

                    panel.Children.Add(new TextBlock
                    {
                        Text = $"🏋 {abonement.Visits} занять",
                        FontSize = 15,
                        Margin = new Thickness(0, 5, 0, 15)
                    });

                    panel.Children.Add(new Separator());

                    panel.Children.Add(new TextBlock
                    {
                        Text = abonement.Description,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 15, 0, 20),
                        FontSize = 15,
                        Foreground = Brushes.Gray
                    });

                    // Контейнер для двох кнопок
                    StackPanel buttonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 15, 0, 0)
                    };

                    // Кнопка "Подати заявку"
                    Button requestButton = new Button
                    {
                        Content = "💖 Подати заявку",
                        Width = 145,
                        Height = 42,
                        Margin = new Thickness(0, 0, 10, 0),
                        Style = (Style)FindResource("MenuButtonStyle"),
                        Tag = abonement
                    };

                    requestButton.Click += RequestButton_Click;

                    // Кнопка "Придбати"
                    Button buyButton = new Button
                    {
                        Content = "💳 Придбати",
                        Width = 145,
                        Height = 42,
                        Style = (Style)FindResource("PrimaryButtonStyle"),
                        Tag = abonement
                    };

                    buyButton.Click += BuyButton_Click;

                    buttonPanel.Children.Add(requestButton);
                    buttonPanel.Children.Add(buyButton);

                    panel.Children.Add(buttonPanel);

                    card.Child = panel;

                    CardsPanel.Children.Add(card);
                }
            }
        }
        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser == null)
            {
                MessageBox.Show(
                    "Для подання заявки необхідно увійти в акаунт.",
                    "Необхідна авторизація",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                NavigationService.Navigate(new LoginPage());
                return;
            }

            Abonement abonement = (Abonement)((Button)sender).Tag;

            MessageBox.Show(
                $"Заявку на абонемент \"{abonement.Name}\" успішно відправлено.\n\nНаш адміністратор зв'яжеться з вами найближчим часом.",
                "Заявку відправлено",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser == null)
            {
                MessageBox.Show(
                    "Для придбання абонемента необхідно увійти в акаунт.",
                    "Необхідна авторизація",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                NavigationService.Navigate(new LoginPage());
                return;
            }

            Abonement abonement =
                (Abonement)((Button)sender).Tag;

            using (AppDbContext db = new AppDbContext())
            {
                UserAbonement oldAbonement =
                    db.UserAbonement.FirstOrDefault(x =>
                        x.UserID == Session.CurrentUser.UserID);

                if (oldAbonement != null)
                {
                    MessageBox.Show(
                        "У вас вже є активний абонемент.",
                        "Увага",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    return;
                }

                UserAbonement newAbonement =
                    new UserAbonement();

                newAbonement.UserID = Session.CurrentUser.UserID;
                newAbonement.AbonementID = abonement.AbonementID;

                newAbonement.StartDate =
                    DateTime.Today.ToString("dd.MM.yyyy");

                newAbonement.EndDate =
                    DateTime.Today
                    .AddDays(abonement.DurationDays)
                    .ToString("dd.MM.yyyy");

                newAbonement.RemainingVisits =
                    abonement.Visits;

                db.UserAbonement.Add(newAbonement);

                db.SaveChanges();
            }

            MessageBox.Show(
                $"Вітаємо!\n\nАбонемент \"{abonement.Name}\" успішно придбано.",
                "Успіх",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            LoadCards();
        }
    }
}