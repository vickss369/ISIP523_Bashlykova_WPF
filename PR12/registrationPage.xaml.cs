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
        private bool isLoginMode = true;

        public registrationPage()
        {
            InitializeComponent();
        }

        private bool IsInputValid()
        {
            bool nameOK = !string.IsNullOrWhiteSpace(userNameTB.Text)
                       && userNameTB.Text.Length >= 2
                       && !userNameTB.Text.Any(char.IsDigit);

            bool passwordOK = !string.IsNullOrWhiteSpace(passwordPB.Password)
                           && passwordPB.Password.Length >= 5
                           && passwordPB.Password.Any(char.IsDigit);

            return nameOK && passwordOK;
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Пожалуйста, корректно заполните все поля.", "Ошибка");
                return;
            }

            string login = userNameTB.Text.Trim();
            string pass = passwordPB.Password;

            if (isLoginMode)
            {
                var user = Core.Context.Users.FirstOrDefault(u => u.UserName == login);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка");
                    passwordPB.Password = "";
                    return;
                }

                if (user.Password == pass)
                {
                    Users.CurrentLogin = login;
                    Users.CurrentPassword = pass;
                    MessageBox.Show("Вы вошли!");
                    NavigationService.Navigate(new filmsPage());
                }
                else
                {
                    MessageBox.Show("Неверный пароль.", "Ошибка");
                }

                passwordPB.Password = ""; 
            }
            else
            {
                if (Core.Context.Users.Any(u => u.UserName == login))
                {
                    MessageBox.Show("Такой логин уже занят!", "Ошибка");
                    passwordPB.Password = "";
                    return;
                }

                Users newUser = new Users
                {
                    UserName = login,
                    Password = pass
                };

                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                Users.CurrentLogin = login;
                Users.CurrentPassword = pass;

                MessageBox.Show("Регистрация прошла успешно!");
                NavigationService.Navigate(new filmsPage());
            }
        }

        private void SwitchMode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLoginMode = !isLoginMode;
            UpdateUIForMode();
            userNameTB.Text = "";
            passwordPB.Password = "";
            userNameTB.Focus();
        }

        private void UpdateUIForMode()
        {
            if (isLoginMode)
            {
                modeTitle.Text = "ВХОД";
                entrBtn.Content = "Войти";
                switchModeText.Text = "Ещё нет аккаунта? Зарегистрироваться";
            }
            else
            {
                modeTitle.Text = "РЕГИСТРАЦИЯ";
                entrBtn.Content = "Зарегистрироваться";
                switchModeText.Text = "Уже есть аккаунт? Войти";
            }
        }

        private void SwitchMode_MouseEnter(object sender, MouseEventArgs e)
        {
            switchModeText.Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 140));
        }

        private void SwitchMode_MouseLeave(object sender, MouseEventArgs e)
        {
            switchModeText.Foreground = new SolidColorBrush(Color.FromRgb(58, 33, 25));
        }
    }
}
