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
    /// Логика взаимодействия для registrationPage.xaml
    /// </summary>
    public partial class registrationPage : Page
    {
        public registrationPage()
        {
            InitializeComponent();
        }

        private bool IsInputValid()
        {
            bool nameOK = !string.IsNullOrWhiteSpace(userNameTB.Text) && userNameTB.Text.Length >= 2 && !userNameTB.Text.Any(char.IsDigit);
            bool passwordOK = !string.IsNullOrWhiteSpace(passwordTB.Text) && passwordTB.Text.Length >= 5 && passwordTB.Text.Any(char.IsDigit);

            return nameOK && passwordOK;
        }

        private void enterFilms_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Пожалуйста, корректно заполните все поля.", "Ошибка");
                return;
            }

            Users user = new Users()
            {
                UserName = userNameTB.Text,
                Password = passwordTB.Text,
            };
            Core.Context.Users.Add(user);
            Core.Context.SaveChanges();

            NavigationService.Navigate(new filmsPage());
        }
    }
}
