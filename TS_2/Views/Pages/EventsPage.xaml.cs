using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TS_2.Database;
using TS_2.Helpers;
using TS_2.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TS_2.Views.Pages
{
    public partial class EventsPage : Page
    {
        public EventsPage()
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

                        bool isRegistered = db.EventsRegistration
                            .Any(r =>
                                r.EventID == item.EventsID &&
                                r.UserID == Session.CurrentUser.UserID);

                        item.ParticipantsInfo =
                            $"{registeredCount} / {item.MaxParticipants} учасників";

                        item.PriceText =
                            item.Price == 0
                                ? "Безкоштовно"
                                : $"{item.Price:0.##} грн";

                        if (registeredCount >= item.MaxParticipants)
                        {
                            item.ButtonText = "Місць немає";
                        }
                        else if (isRegistered)
                        {
                            item.ButtonText = "Скасувати запис";
                        }
                        else
                        {
                            item.ButtonText = "Записатися";
                        }
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

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
                button.Tag is not Event selectedEvent)
                return;

            try
            {
                using (var db = new AppDbContext())
                {
                    var registration = db.EventsRegistration
                        .FirstOrDefault(r =>
                            r.EventID == selectedEvent.EventsID &&
                            r.UserID == Session.CurrentUser.UserID);

                    // Якщо користувач уже записаний — скасовуємо запис
                    if (registration != null)
                    {
                        db.EventsRegistration.Remove(registration);
                        db.SaveChanges();

                        MessageBox.Show(
                            "Запис на подію скасовано.",
                            "Події",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        // Перевіряємо кількість місць ще раз
                        int registeredCount = db.EventsRegistration
                            .Count(r => r.EventID == selectedEvent.EventsID);

                        if (registeredCount >= selectedEvent.MaxParticipants)
                        {
                            MessageBox.Show(
                                "На жаль, вільних місць більше немає.",
                                "Події",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                            LoadEvents();
                            return;
                        }

                        var newRegistration = new EventsRegistration
                        {
                            UserID = Session.CurrentUser.UserID,
                            EventID = selectedEvent.EventsID,
                            RegistrationDate = DateTime.Now.ToString("dd.MM.yyyy")
                        };

                        db.EventsRegistration.Add(newRegistration);
                        db.SaveChanges();

                        MessageBox.Show(
                            "Ви успішно записалися на подію!",
                            "Події",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }

                LoadEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося виконати операцію.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}