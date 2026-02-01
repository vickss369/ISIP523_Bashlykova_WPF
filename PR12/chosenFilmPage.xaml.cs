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

            UpdateButtonsVis();
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
                    sessionsList.ItemsSource = currentFilm.Sessions.OrderBy(s => s.SessionDate).ToList();
                }
                else
                {
                    sessionsList.ItemsSource = null;
                }

                if (!string.IsNullOrEmpty(currentFilm.ImagePath))
                {
                    FilmImage.Source = new BitmapImage(new Uri(currentFilm.ImagePath, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void backToFilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateButtonsVis()
        {
            if (Users.IsUserValid())
            {
                toregistrBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                toregistrBtn.Visibility = Visibility.Visible;
            }
        }

        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new registrationPage());
        }

        private void ChooseSession_Click(object sender, RoutedEventArgs e)
        {
            if (!Users.IsUserValid())
            {
                MessageBox.Show("Сначала нужно зарегистрироваться или войти, чтобы выбрать сеанс!");
                NavigationService.Navigate(new registrationPage());
                return;
            }

            Button btn = sender as Button;
            if (btn == null) return;

            Sessions selectedSession = btn.DataContext as Sessions;
            if (selectedSession == null) return;

            NavigationService.Navigate(new chosenSessionPage(selectedSession));
        }
    }
}
