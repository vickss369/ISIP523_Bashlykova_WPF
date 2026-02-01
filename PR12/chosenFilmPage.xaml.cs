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
    /// Логика взаимодействия для chosenFilmPage.xaml
    /// </summary>
    public partial class chosenFilmPage : Page
    {
        private Films currentFilm;

        public chosenFilmPage(Films cf)
        {
            InitializeComponent();
            currentFilm = cf;

            LoadFilmInfo();
        }

        private void LoadFilmInfo()
        {
            if (currentFilm != null)
            {
                FilmNameTB.Text = currentFilm.FilmName;
                DescriptionTB.Text = currentFilm.Description;
                RatingTB.Text = "Рейтинг: " + currentFilm.FilmRating;
                AgeRatingTB.Text = "Возрастной рейтинг: " + (currentFilm.AgeRating != null ? currentFilm.AgeRating.Nomination : "-");
                StartDateTB.Text = "Дата начала показа: " + currentFilm.StartDate.ToShortDateString();

                if (currentFilm.FilmsGenres != null && currentFilm.FilmsGenres.Count > 0)
                {
                    var genres = currentFilm.FilmsGenres.Where(fg => fg.Genres != null).Select(fg => fg.Genres.Nomanation);
                    GenreTB.Text = "Жанр: " + string.Join(", ", genres);
                }
                else
                {
                    GenreTB.Text = "Жанр: -";
                }

                if (currentFilm.Sessions != null && currentFilm.Sessions.Count > 0)
                {
                    var sessions = currentFilm.Sessions.Select(s => s.SessionDate.ToShortDateString()).ToList();
                    SessionsTB.Text = "Даты сеансов: " + string.Join(", ", sessions);
                }
                else
                {
                    SessionsTB.Text = "Даты сеансов: -";
                }

                if (!string.IsNullOrEmpty(currentFilm.ImagePath))
                {
                    FilmImage.Source = new BitmapImage(new Uri(currentFilm.ImagePath, UriKind.RelativeOrAbsolute));
                }
            }
        }


        private void buyTicket_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new buyTicketPage(currentFilm));
        }

        private void backToFilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new registrationPage());
        }
    }
}
