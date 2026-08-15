using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TS_2.Database;
using TS_2.Models;

namespace TS_2.Views.Pages
{
    public partial class EventEditWindow : Page
    {
        private readonly int? _eventId;

        // Додавання нової події
        public EventEditWindow()
        {
            InitializeComponent();

            PageTitle.Text = "⭐ Додати подію";
            DatePicker.SelectedDate = DateTime.Today;
        }

        // Редагування існуючої події
        public EventEditWindow(int eventId)
        {
            InitializeComponent();

            _eventId = eventId;

            PageTitle.Text = "✏️ Редагувати подію";

            LoadEvent();
        }

        private void LoadEvent()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var eventItem = db.Event
                        .FirstOrDefault(e => e.EventsID == _eventId.Value);

                    if (eventItem == null)
                    {
                        MessageBox.Show(
                            "Подію не знайдено.",
                            "Помилка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        NavigationService.GoBack();
                        return;
                    }

                    TitleBox.Text = eventItem.Title;
                    DescriptionBox.Text = eventItem.Description;
                    PlaceBox.Text = eventItem.Place;
                    MaxParticipantsBox.Text =
                        eventItem.MaxParticipants.ToString();
                    PriceBox.Text =
                        eventItem.Price.ToString();

                    // Перетворення string → DateTime
                    if (DateTime.TryParse(
                        eventItem.Date,
                        out DateTime eventDate))
                    {
                        DatePicker.SelectedDate = eventDate;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося завантажити подію.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string description = DescriptionBox.Text.Trim();
            string place = PlaceBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show(
                    "Введіть назву події.",
                    "Перевірка даних",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Оберіть дату події.",
                    "Перевірка даних",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(
                    MaxParticipantsBox.Text.Trim(),
                    out int maxParticipants) ||
                maxParticipants <= 0)
            {
                MessageBox.Show(
                    "Вкажіть коректну кількість учасників.",
                    "Перевірка даних",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(
                    PriceBox.Text.Trim(),
                    out int price) ||
                price < 0)
            {
                MessageBox.Show(
                    "Вкажіть коректну вартість події.",
                    "Перевірка даних",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            string date =
                DatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");

            try
            {
                using (var db = new AppDbContext())
                {
                    if (_eventId.HasValue)
                    {
                        // Редагування
                        var eventItem = db.Event
                            .FirstOrDefault(
                                e => e.EventsID == _eventId.Value);

                        if (eventItem == null)
                        {
                            MessageBox.Show(
                                "Подію не знайдено.",
                                "Помилка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                            return;
                        }

                        eventItem.Title = title;
                        eventItem.Description = description;
                        eventItem.Date = date;
                        eventItem.Place = place;
                        eventItem.MaxParticipants = maxParticipants;
                        eventItem.Price = price;
                    }
                    else
                    {
                        // Додавання
                        var newEvent = new Event
                        {
                            Title = title,
                            Description = description,
                            Date = date,
                            Place = place,
                            MaxParticipants = maxParticipants,
                            Price = price
                        };

                        db.Event.Add(newEvent);
                    }

                    db.SaveChanges();
                }

                MessageBox.Show(
                    _eventId.HasValue
                        ? "Подію успішно відредаговано."
                        : "Подію успішно додано.",
                    "Події",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не вдалося зберегти подію.\n\n{ex.Message}",
                    "Помилка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}