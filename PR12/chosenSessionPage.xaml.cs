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
        private List<SessionPlace> selectedPlaces = new List<SessionPlace>();

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
            AgeRatingTB.Text = currentSession.Films.AgeRating.Nomination;
            SessionTB.Text = "Сеанс: " + currentSession.SessionDate.ToString("dd.MM.yyyy") + ", Зал: " + currentSession.Halls.HallRating.Nomination;
        }

        private void LoadPlaces()
        {
            var placesInHall = Core.Context.Places.Where(p => p.HallID == currentSession.HallID).ToList();
            var sessionPlaces = Core.Context.SessionPlace.Where(s => s.SessionID == currentSession.SessionID).ToList();

            var displayPlaces = placesInHall.Select(p =>
            {
                var sp = sessionPlaces.FirstOrDefault(s => s.PlaceID == p.PlaceID);
                if (sp == null)
                {
                    sp = new SessionPlace
                    {
                        SessionID = currentSession.SessionID,
                        PlaceID = p.PlaceID,
                        Status = false,
                        Places = p,
                        Sessions = currentSession
                    };
                }

                bool isOccupied = sp.Status;
                return new
                {
                    PlaceNumber = p.PlaceNumber,
                    SessionPlace = sp,
                    IsOccupied = isOccupied,
                    Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(isOccupied ? "#6B212C" : "#849DBB")),
                    IsEnabled = !isOccupied
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

            SessionPlace sp = place.SessionPlace;

            if (selectedPlaces.Contains(sp))
            {
                selectedPlaces.Remove(sp);
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D2E2EC"));
            }
            else
            {
                selectedPlaces.Add(sp);
                btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9A883"));
            }
            UpdateSelectedPanel();
        }

        private void UpdateSelectedPanel()
        {
            if (selectedPlaces.Count == 0)
            {
                selectedPlacePanel.Visibility = Visibility.Collapsed;
                return;
            }

            selectedPlacePanel.Visibility = Visibility.Visible;
            var numbers = selectedPlaces.Select(p => p.Places.PlaceNumber).ToList();

            SelectedPlaceTB.Text = "Места: " + string.Join(", ", numbers);
            SelectedHallTB.Text = "Зал: " + currentSession.Halls.HallRating.Nomination;
            SelectedDateTB.Text = "Дата: " + currentSession.SessionDate.ToString("dd.MM.yyyy");
        }

        private void delPlaces_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaces.Count == 0) return;

            selectedPlaces.Clear();
            selectedPlacePanel.Visibility = Visibility.Collapsed;

            LoadPlaces();
        }

        private void buyTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaces.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно место!");
                return;
            }

            NavigationService.Navigate(new buyTicketPage(selectedPlaces, currentSession));
        }

        private void goToSession_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
