using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TS_2.Database;
using TS_2.Models;

namespace TS_2.Views.Pages
{
    public partial class AdminEventsPage : Page
    {
        public AdminEventsPage()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var events = db.Event
                        .OrderBy(e => e.Date)
                        .ToList();

                    foreach (var item in events)
                    {
                        int registeredCount = db.EventsRegistration
                            .Count(r => r.EventID == item.EventsID);

                        item.ParticipantsInfo =
                            $"{registeredCount} / {item.MaxParticipants} учасників";

                        item.PriceText =
                            item.Price == 0
                                ? "Безкоштовно"
                                : $"{item.Price:0.##} грн";
                    }

                    EventsList.ItemsSource = events;

                    EmptyText.Visibility =
                        events.Count == 0
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося завантажити події.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventEditWindow());
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
                button.Tag is not Event selectedEvent)
                return;

            NavigationService.Navigate(
                new EventEditWindow(selectedEvent.EventsID));
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
                button.Tag is not Event selectedEvent)
                return;

            var result = MessageBox.Show(
                $"Ви дійсно хочете видалити подію «{selectedEvent.Title}»?",
                "Видалення події",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new AppDbContext())
                {
                    var eventToDelete = db.Event
                        .FirstOrDefault(ev => ev.EventsID == selectedEvent.EventsID);

                    if (eventToDelete == null)
                        return;

                    var registrations = db.EventsRegistration
                        .Where(r => r.EventID == selectedEvent.EventsID)
                        .ToList();

                    db.EventsRegistration.RemoveRange(registrations);
                    db.Event.Remove(eventToDelete);

                    db.SaveChanges();
                }

                MessageBox.Show(
                    "Подію успішно видалено.",
                    "Події",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося видалити подію.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Guests_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
                button.Tag is not Event selectedEvent)
                return;

            NavigationService.Navigate(
                new EventGuestsPage(selectedEvent.EventsID));
        }
    }
}