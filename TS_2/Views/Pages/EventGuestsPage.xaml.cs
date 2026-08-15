using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TS_2.Database;
using TS_2.Models;

namespace TS_2.Views.Pages
{
    public partial class EventGuestsPage : Page
    {
        private readonly int _eventId;

        public EventGuestsPage(int eventId)
        {
            InitializeComponent();

            _eventId = eventId;

            LoadGuests();
        }

        private void LoadGuests()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var eventItem = db.Event
                        .FirstOrDefault(e => e.EventsID == _eventId);

                    if (eventItem == null)
                    {
                        MessageBox.Show("Подію не знайдено.");
                        return;
                    }

                    TitleText.Text = "⭐ Гості події: " + eventItem.Title;

                    var guests = (
                        from registration in db.EventsRegistration
                        join user in db.Users
                            on registration.UserID equals user.UserID
                        where registration.EventID == _eventId
                        select user
                    ).ToList();

                    GuestsList.ItemsSource = guests;

                    CountText.Text =
                        $"Зареєстровано гостей: {guests.Count} / {eventItem.MaxParticipants}";

                    EmptyText.Visibility =
                        guests.Count == 0
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося завантажити список гостей.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Guest_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border ||
                border.DataContext is not User selectedUser)
                return;

            NavigationService.Navigate(
                new AdminUserDetailsPage(selectedUser));
        }
    }
}