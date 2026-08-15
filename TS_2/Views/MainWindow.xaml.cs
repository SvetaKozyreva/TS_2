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
using System.Windows.Shapes;
using TS_2.Helpers;
using TS_2.Models;
using TS_2.Views.Pages;



namespace TS_2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(new HomePage(),
         "Головна",
         HomeButton);

        }
        public Frame Frame => MainFrame;
        public TextBlock PageTitleControl => PageTitle;
        public User CurrentUser { get; set; }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new HomePage(),
         "Головна",
         HomeButton);
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new SchedulePage(),
             "Розклад",
             ScheduleButton);
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new EventsPage(),
             "Події",
             EventsButton);
        }

        private void AbonementsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new AbonementsPage(),
             "Абонементи",
             AbonementsButton);
        }
        private void OpenPage(Page page, string title, Button activeButton)
        {
            MainFrame.Navigate(page);

            PageTitle.Text = title;

            HomeButton.Style = (Style)FindResource("MenuButtonStyle");
            ScheduleButton.Style = (Style)FindResource("MenuButtonStyle");
            EventsButton.Style = (Style)FindResource("MenuButtonStyle");
            AbonementsButton.Style = (Style)FindResource("MenuButtonStyle");
            AdminButton.Style = (Style)FindResource("MenuButtonStyle");

            if (activeButton != null)
            {
                activeButton.Style = (Style)FindResource("PrimaryButtonStyle");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser == null)
            {
                MainFrame.Navigate(new LoginPage());
            }
            else
            {
                MainFrame.Navigate(new ProfilePage());
            }
        }
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new AdminPage(),
             "Адмін-панель",
             AdminButton);
        }
        public void UpdateUser()
        {
            if (Session.CurrentUser != null)
            {
                LoginButton.Content = "👤 " + Session.CurrentUser.FullName;

                // Показываем кнопку админ-панели только админу
                if (Session.CurrentUser.Role == "Адміністратор")
                {
                    AdminButton.Visibility = Visibility.Visible;
                }
                else
                {
                    AdminButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LoginButton.Content = "👤 Вхід";
                AdminButton.Visibility = Visibility.Collapsed;
            }
        }
        public void Navigate(Page page, string title)
        {
            MainFrame.Navigate(page);
            PageTitle.Text = title;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                UpdateNavigationButtons();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
            {
                MainFrame.GoForward();
                UpdateNavigationButtons();
            }
        }

        private void UpdateNavigationButtons()
        {
            BackButton.IsEnabled = MainFrame.CanGoBack;
            ForwardButton.IsEnabled = MainFrame.CanGoForward;
        }



    }

}
