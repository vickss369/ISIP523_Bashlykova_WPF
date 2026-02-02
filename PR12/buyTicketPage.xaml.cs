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
            if (selectedPlaces.Count == 0)
            {
                MessageBox.Show("Выберите места!");
                return;
            }

            MessageBox.Show("Билет успешно куплен!");
            // тут сохранить в БД будет когда-то

            NavigationService.Navigate(new MainPage());
        }
    }

}
