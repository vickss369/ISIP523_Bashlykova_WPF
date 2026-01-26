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
        public filmsPage()
        {
            InitializeComponent();

            AddFilms();
            LoadFilms();
        }

        private void goToAkk_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new accountPage());
        }

        private void AddFilms()
        {
            if (!Core.Context.Films.Any())
            {
                var f1 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
                };

                var f2 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
                };

                var f3 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
                };

                var f4 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
                };

                var f5 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
                };

                var f6 = new Films
                {
                    FilmName = "Майор Гром: Игра",
                    Description = "У бесстрашного защитника Петербурга майора Игоря Грома — новый враг, называющий себя Призраком. Он предлагает Грому сыграть в опасную игру, ставка в которой — жизни обычных людей.",
                    FilmRating = 7.5,
                    StartDate = new System.DateTime(),
                    AgeRatingID = 4,
                    ImagePath = "Images/grom.png"
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
            var list = Core.Context.Films.ToList();
            filmsList.ItemsSource = list;
        }


        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new registrationPage());
        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
