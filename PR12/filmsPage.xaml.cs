using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
    /// Логика взаимодействия для filmsPage.xaml
    /// </summary>
    public partial class filmsPage : Page
    {
        private Films chosenFilm;

        private List<Films> allFilms;
        private List<string> searchCriteria = new List<string>() { "Название", "Рейтинг" };

        public filmsPage()
        {
            InitializeComponent();

            AddFilms();
            LoadFilms();

            SortCB.ItemsSource = searchCriteria;

            SearchTB.TextChanged += (s, e) => FilterFilms();
            SortCB.SelectionChanged += (s, e) => FilterFilms();
        }

        /*public filmsPage(Films cf) //для возврата со страницы покупки билета
        {
            InitializeComponent();
            chosenFilm = cf; 
        }*/

        private void goToAkk_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new accountPage());
        }

        private void AddFilms()
        {
            if (!Core.Context.Films.Any())
            {
                Films f1 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new DateTime(2024, 05, 23),
                    AgeRatingID = 3,
                    ImagePath = "Images/grom.png"
                };

                var f2 = new Films
                {
                    FilmName = "Властелин Колец: Возвращение короля",
                    Description = "Армия Саурона осаждает Минас-Тирит, пока Фродо и Сэм приближаются к Роковой горе, чтобы уничтожить Кольцо. В это время Арагорн возглавляет свободные народы Средиземья в решающей битве.",
                    FilmRating = 8.7,
                    StartDate = new DateTime(2004, 01, 22),
                    AgeRatingID = 3,
                    ImagePath = "Images/lordOfTheRings.png"
                };

                var f3 = new Films
                {
                    FilmName = "Сияние",
                    Description = "Джек Торренс с женой и сыном приезжает в элегантный отдалённый отель, чтобы работать смотрителем во время мертвого сезона. Вскоре он и его семья становятся жертвами мрака, сотканного из преступного кошмара отеля.",
                    FilmRating = 7.9,
                    StartDate = new DateTime(1980, 05, 23),
                    AgeRatingID = 4,
                    ImagePath = "Images/shining.png"
                };

                var f4 = new Films
                {
                    FilmName = "Чебурашка",
                    Description = "Чебурашка оказывается в домике нелюдимого старика-садовника Геннадия, который из вредности решает оставить его жить у себя, так как местная богачка жаждет заполучить необычного зверя для своей избалованной внучки.",
                    FilmRating = 7.5,
                    StartDate = new DateTime(2023, 01, 01),
                    AgeRatingID = 2,
                    ImagePath = "Images/chebyrashka.png"
                };

                var f5 = new Films
                {
                    FilmName = "Анна Каренина",
                    Description = "Замужняя светская дама Анна Каренина жертвует репутацией в свете ради трагической любви к юному офицеру.",
                    FilmRating = 8.6,
                    StartDate = new DateTime(1967, 11, 06),
                    AgeRatingID = 4,
                    ImagePath = "Images/AnnaKarenina.png"
                };

                var f6 = new Films
                {
                    FilmName = "Сумерки. Сага. Затмение",
                    Description = "Сиэтл охвачен чередой таинственных убийств, а обуреваемая жаждой мести вампирша продолжает поиски Беллы, снова оказавшейся в смертельной опасности.",
                    FilmRating = 6.3,
                    StartDate = new DateTime(2010, 06, 30),
                    AgeRatingID = 3,
                    ImagePath = "Images/twinlight.png"
                };

                Core.Context.Films.Add(f1);
                Core.Context.Films.Add(f2);
                Core.Context.Films.Add(f3);
                Core.Context.Films.Add(f4);
                Core.Context.Films.Add(f5);
                Core.Context.Films.Add(f6);

                Core.Context.SaveChanges();
            }
        }

        private void LoadFilms()
        {
            allFilms = Core.Context.Films.ToList();
            filmsList.ItemsSource = allFilms;
        }

        private void FilterFilms()
        {
            string searchText = SearchTB.Text.ToLower().Trim();

            var filtered = allFilms.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(f => f.FilmName.ToLower().Contains(searchText));
            }

            var selectedItem = SortCB.SelectedItem as string ?? "Название";
            if (selectedItem == "Название")
            {
                filtered = filtered.OrderBy(f => f.FilmName);
            }
            else if (selectedItem == "Рейтинг")
            {
                filtered = filtered.OrderByDescending(f => f.FilmRating);
            }

            filmsList.ItemsSource = filtered.ToList();
        }

        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterFilms();
        }

        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new registrationPage());
        }

        private void choseFilmBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null) return;

            chosenFilm = btn.DataContext as Films;

            if (chosenFilm == null)
            {
                MessageBox.Show("Не удалось выбрать фильм");
                return;
            }

            NavigationService.Navigate(new chosenFilmPage(chosenFilm));
        }

    }
}
