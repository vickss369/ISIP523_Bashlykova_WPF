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
    /// Логика взаимодействия для buyTicketPage.xaml
    /// </summary>
    public partial class buyTicketPage : Page
    {
        private List<SessionPlace> selectedPlaces;
        private Sessions currentSession;

        public buyTicketPage(List<SessionPlace> places, Sessions session)
        {
            InitializeComponent();

            selectedPlaces = places;
            currentSession = session;

            LoadTicketInfo();
        }

        private void LoadTicketInfo()
        {
            FilmNameTB.Text = "Фильм: " + currentSession.Films.FilmName;
            HallNumTB.Text = "Зал: " + currentSession.Halls.HallRating.Nomination;
            SessionDateTB.Text = "Дата сеанса: " + currentSession.SessionDate.ToString("dd.MM.yyyy");

            var placeNumbers = selectedPlaces.Select(p => p.Places.PlaceNumber).ToList();
            SelectedPlaceTB.Text ="Места: " + string.Join(", ", placeNumbers);

            double basePrice = currentSession.Halls.HallRating.PlacePrice;
            double finalPrice = basePrice * 1.5 * selectedPlaces.Count;
            PriceTB.Text = "Стоимость: " + finalPrice.ToString("0.00") + "₽";
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlaces == null || selectedPlaces.Count == 0)
            {
                MessageBox.Show("Выберите места!");
                return;
            }

            var currentUser = Core.Context.Users.FirstOrDefault(u => u.UserName == Users.CurrentLogin && u.Password == Users.CurrentPassword);
            if (currentUser == null)
            {
                MessageBox.Show("Ошибка авторизации!");
                NavigationService.Navigate(new registrationPage());
                return;
            }

            double basePrice = currentSession.Halls.HallRating.PlacePrice;
            double oneTicketPrice = basePrice * 1.5;
            foreach (var sp in selectedPlaces)
            {
                var sessionPlace = Core.Context.SessionPlace.FirstOrDefault(x => x.SessionID == currentSession.SessionID && x.PlaceID == sp.PlaceID);
                if (sessionPlace == null)
                {
                    sessionPlace = new SessionPlace
                    {
                        SessionID = currentSession.SessionID,
                        PlaceID = sp.PlaceID,
                        Status = true
                    };
                    Core.Context.SessionPlace.Add(sessionPlace);
                }
                else
                {
                    sessionPlace.Status = true;
                }


                Tickets ticket = new Tickets
                {
                    SessionID = currentSession.SessionID,
                    UserID = currentUser.UserID,
                    PlaceID = sp.PlaceID,
                    Price = oneTicketPrice
                };
                Core.Context.Tickets.Add(ticket);
            }
            Core.Context.SaveChanges();

            MessageBox.Show("Покупка успешно завершена!");
            NavigationService.Navigate(new MainPage());
        }
    }
}
