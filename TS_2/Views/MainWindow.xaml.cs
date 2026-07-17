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
            // Открываем страницу
            MainFrame.Navigate(page);

            // Меняем заголовок
            PageTitle.Text = title;

            // Снимаем подсветку со всех кнопок
            HomeButton.Style = (Style)FindResource("MenuButtonStyle");
            ScheduleButton.Style = (Style)FindResource("MenuButtonStyle");
            EventsButton.Style = (Style)FindResource("MenuButtonStyle");
            AbonementsButton.Style = (Style)FindResource("MenuButtonStyle");

            // Подсвечиваем активную кнопку
            activeButton.Style = (Style)FindResource("PrimaryButtonStyle");
        }
    }

}
