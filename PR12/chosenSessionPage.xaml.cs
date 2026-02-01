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

namespace PR12
{
    /// <summary>
    /// Логика взаимодействия для chosenSessionPage.xaml
    /// </summary>
    public partial class chosenSessionPage : Page
    {
        private Sessions currentSession;

        public chosenSessionPage(Sessions session)
        {
            InitializeComponent();
            currentSession = session;

            if (!Users.IsUserValid())
            {
                MessageBox.Show("Сначала зарегистрируйтесь или войдите!");
                NavigationService.Navigate(new registrationPage());
                return;
            }
            LoadSessionInfo();
            LoadPlaces();
        }

        private void LoadSessionInfo()
        {
            FilmNameTB.Text = currentSession.Films.FilmName;
            AgeRatingTB.Text = "Возрастной рейтинг: " + (currentSession.Films.AgeRating?.Nomination ?? "-");
            StartDateTB.Text = "Дата начала показа: " + currentSession.Films.StartDate.ToShortDateString();
            SessionTB.Text = "Сеанс: " + currentSession.SessionDate.ToString("dd.MM.yyyy HH:mm") + ", Зал: " + currentSession.Halls.HallRating.Nomination;
        }

        private void LoadPlaces()
        {
            var placesInHall = Core.Context.Places
                .Where(p => p.HallID == currentSession.HallID)
                .ToList();

            var sessionPlaces = currentSession.SessionPlace.ToList();

            var displayPlaces = placesInHall.Select(p =>
            {
                var sp = sessionPlaces.FirstOrDefault(s => s.PlaceID == p.PlaceID);
                bool isOccupied = sp != null && sp.Status;

                return new
                {
                    PlaceID = p.PlaceID,
                    PlaceNumber = p.PlaceNumber,
                    StatusColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(isOccupied ? "#442D1C" : "#849DBB")),
                    IsOccupied = isOccupied,
                    SessionPlaceObj = sp
                };
            }).ToList();

            placesList.ItemsSource = displayPlaces;
        }

        private void Place_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            dynamic place = btn.DataContext;
            if (place == null) return;

            if (place.IsOccupied)
            {
                MessageBox.Show("Это место уже занято!");
                return;
            }

            selectedPlacePanel.Visibility = Visibility.Visible;
            SelectedPlaceTB.Text = "Место: " + place.PlaceNumber;
            SelectedHallTB.Text = "Зал: " + currentSession.Halls.HallRating.Nomination;
            SelectedDateTB.Text = "Дата сеанса: " + currentSession.SessionDate.ToString("dd.MM.yyyy");

            buyTicketBtn.Tag = place.SessionPlaceObj;
        }

        private void buyTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag is SessionPlace sp)
            {
                NavigationService.Navigate(new buyTicketPage(sp));
            }
        }
        private void goToSession_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
