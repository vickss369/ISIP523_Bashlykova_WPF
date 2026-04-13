using PR12.Classes;
using PR12;
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

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для enterPage.xaml
    /// </summary>
    public partial class enterPage : Page
    {
        private bool isLoginMode = true;

        public enterPage()
        {
            InitializeComponent();
            UpdateUIForMode();
        }

        private bool IsInputValid()
        {
            if (isLoginMode)
            {
                bool loginOK = !string.IsNullOrWhiteSpace(loginTB.Text) && loginTB.Text.Length >= 2;
                bool passwordOK = !string.IsNullOrWhiteSpace(passwordPB.Password) && passwordPB.Password.Length >= 5;

                return loginOK && passwordOK;
            }
            else
            {
                bool fullNameOK = !string.IsNullOrWhiteSpace(fullNameTB.Text)
                                  && fullNameTB.Text.Length >= 5
                                  && !fullNameTB.Text.Any(char.IsDigit);

                bool loginOK = !string.IsNullOrWhiteSpace(loginTB.Text)
                               && loginTB.Text.Length >= 2;

                bool passwordOK = !string.IsNullOrWhiteSpace(passwordPB.Password)
                                  && passwordPB.Password.Length >= 5
                                  && passwordPB.Password.Any(char.IsDigit);

                bool phoneOK = !string.IsNullOrWhiteSpace(phoneTB.Text)
                               && phoneTB.Text.All(c => char.IsDigit(c) || c == '+');

                return fullNameOK && loginOK && passwordOK && phoneOK;
            }
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Проверьте правильность заполнения полей.");
                return;
            }

            string login = loginTB.Text.Trim();
            string pass = passwordPB.Password;

            if (isLoginMode)
            {
                var user = Core.Context.User.FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.");
                    passwordPB.Password = "";
                    return;
                }

                if (user.Password != pass)
                {
                    MessageBox.Show("Неверный пароль.");
                    passwordPB.Password = "";
                    passwordPB.Focus();
                    return;
                }

                if (user.IsFreeze)
                {
                    MessageBox.Show("Пользователь заморожен.");
                    return;
                }

                User.currentUser = user;
                switch (user.RoleID)
                {
                    case 1: NavigationService.Navigate(new clientPage()); break;
                    case 2: NavigationService.Navigate(new masterPage()); break;
                    case 3: NavigationService.Navigate(new managerPage()); break;
                    case 4: NavigationService.Navigate(new adminPage()); break;
                }
            }
            else
            {
                if (Core.Context.User.Any(u => u.Login == login))
                {
                    MessageBox.Show("Логин уже занят.");
                    return;
                }

                User newUser = new User
                {
                    FullName = fullNameTB.Text.Trim(),
                    Login = login,
                    Password = pass,
                    PhoneNumber = phoneTB.Text.Trim(),
                    RoleID = 1,
                    IsFreeze = false
                };

                Core.Context.User.Add(newUser);
                Core.Context.SaveChanges();

                User.currentUser = newUser;
                MessageBox.Show("Регистрация успешна!");
                NavigationService.Navigate(new clientPage());
            }
        }

        private void SwitchMode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLoginMode = !isLoginMode;
            UpdateUIForMode();

            loginTB.Text = "";
            passwordPB.Password = "";
            fullNameTB.Text = "";
            phoneTB.Text = "";
        }

        private void UpdateUIForMode()
        {
            if (isLoginMode)
            {
                modeTitle.Text = "ВХОД";
                entrBtn.Content = "Войти";
                switchModeText.Text = "Ещё нет аккаунта? Зарегистрироваться";

                fullNameTB.Visibility = Visibility.Collapsed;
                fullNameTBl.Visibility = Visibility.Collapsed;
                phoneTB.Visibility = Visibility.Collapsed;
                phoneLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                modeTitle.Text = "РЕГИСТРАЦИЯ";
                entrBtn.Content = "Зарегистрироваться";
                switchModeText.Text = "Уже есть аккаунт? Войти";

                fullNameTB.Visibility = Visibility.Visible;
                fullNameTBl.Visibility = Visibility.Visible;
                phoneTB.Visibility = Visibility.Visible;
                phoneLabel.Visibility = Visibility.Visible;
            }
        }

        private void SwitchMode_Hover(object sender, MouseEventArgs e)
        {
            if (e.RoutedEvent == MouseEnterEvent) switchModeText.Foreground = Brushes.Blue;

            else switchModeText.Foreground = Brushes.Brown;
        }
    }
}
