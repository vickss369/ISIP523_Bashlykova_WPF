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
    /// Логика взаимодействия для accountPage.xaml
    /// </summary>
    public partial class accountPage : Page
    {
        private Users currentUser;

        public accountPage()
        {
            InitializeComponent();
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            currentUser = Core.Context.Users.FirstOrDefault(u => u.UserName == Users.CurrentLogin && u.Password == Users.CurrentPassword);
            if (currentUser == null)
            {
                MessageBox.Show("Сначала войдите или зарегистрируйтесь!");
                NavigationService.Navigate(new registrationPage());
                return;
            }
            UserNameTB.Text = currentUser.UserName;

            var tickets = Core.Context.Tickets.Where(t => t.UserID == currentUser.UserID)
                .Select(t => new
                {
                    FilmName = t.Sessions.Films.FilmName,
                    HallName = t.Sessions.Halls.HallRating.Nomination,
                    SessionDate = t.Sessions.SessionDate,
                    Price = t.Price
                })
                .ToList();

            ticketsList.ItemsSource = tickets;
        }

        private void goTofilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new filmsPage());
        }
    }
}
