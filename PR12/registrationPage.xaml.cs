using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly CaptchaLogic captchaLog = new CaptchaLogic();

        public registrationPage()
        {
            InitializeComponent();
        }

        private bool IsInputValid(string login, string pass)
        {
            bool nameOK = !string.IsNullOrWhiteSpace(login)
                       && login.Length >= 2
                       && !login.Any(char.IsDigit);

            bool passwordOK = !string.IsNullOrWhiteSpace(pass)
                           && pass.Length >= 5
                           && pass.Any(char.IsDigit);

            return nameOK && passwordOK;
        }

        /// <summary>
        /// Выполняет авторизацию пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин зарегистрированного пользователя.</param>
        /// <param name="pass">Пароль пользователя.</param>
        /// <returns>
        /// <c>true</c> — если авторизация прошла успешно и пользователь перенаправлен на filmsPage
        /// <c>false</c> — если авторизация не удалась (неверный логин/пароль, показана капча и т.д.).
        /// </returns>
        public bool Auth(string login, string pass)
        {
            var user = Core.Context.Users.FirstOrDefault(u => u.UserName.Trim().ToLower() == login.Trim().ToLower());
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAndFocusLoginFields();
                return false;
            }

            if (user.Password == pass)
            {
                Users.CurrentLogin = login;
                Users.CurrentPassword = pass;
                MessageBox.Show("Вы вошли!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                if (NavigationService != null)
                {
                    NavigationService.Navigate(new filmsPage());
                }

                ResetAfterSuccess();
                return true;
            }
            else
            {
                captchaLog.RegisterFailedPasswordAttempt(login);
                if (captchaLog.ShouldShowCaptcha())
                {
                    passwordPB.Password = "";
                    ShowCaptcha();
                    MessageBox.Show("Неверный пароль.\nПройдите проверку.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show($"Неверный пароль. Осталось попыток: {3 - captchaLog.failedLoginAttempts}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    passwordPB.Focus();
                }

                passwordPB.Password = "";
                return false;
            }
        }

        /// <summary>
        /// Выполняет регистрацию нового пользователя.
        /// </summary>
        /// <param name="login">Желаемый логин пользователя.</param>
        /// <param name="pass">Пароль пользователя.</param>
        /// <returns>
        /// <c>true</c> — если регистрация прошла успешно и пользователь перенаправлен на filmsPage
        /// <c>false</c> — если регистрация не удалась (логин уже занят).
        /// </returns>
        public bool Reg(string login, string pass)
        {
            if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Пожалуйста, заполните оба поля.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Введите пароль.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsInputValid(login, pass))
            {
                MessageBox.Show("Логин должен содержать минимум 2 символа.\n" +
                                "Пароль должен содержать минимум 5 символов и хотя бы одну цифру.",
                                "Ошибка валидации",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Core.Context.Users.Any(u => u.UserName == login))
            {
                MessageBox.Show("Такой логин уже занят!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
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

            MessageBox.Show("Регистрация прошла успешно!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            if (NavigationService != null)
            {
                NavigationService.Navigate(new filmsPage());
            }

            return true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Вход" / "Регистрация".
        /// В зависимости от текущего режима (<see cref="isLoginMode"/>) вызывает либо авторизацию, либо регистрацию.
        /// </summary>
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            string login = userNameTB.Text.Trim();
            string pass = passwordPB.Password;
            if (!IsInputValid(login, pass))
            {
                MessageBox.Show("Пожалуйста, корректно заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool success;

            if (isLoginMode)
            {
                success = Auth(login, pass);
            }
            else
            {
                success = Reg(login, pass);
            }
        }

        private void ShowCaptcha()
        {
            passwordPB.IsEnabled = false;
            userNameTB.IsEnabled = false;

            captchaContainer.Visibility = Visibility.Visible;
            entrBtn.Visibility = Visibility.Collapsed;
            switchModeText.Visibility = Visibility.Collapsed;

            captchaLog.GenerateCaptchaText();
            captchaImage.Source = captchaLog.CreateCaptchaImage();

            captchaAnswer.Focus();
        }

        private void HideCaptcha()
        {
            captchaContainer.Visibility = Visibility.Collapsed;
            entrBtn.Visibility = Visibility.Visible;
            switchModeText.Visibility = Visibility.Visible;
            captchaAnswer.Text = "";

            passwordPB.IsEnabled = true;
            userNameTB.IsEnabled = true;
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            captchaLog.GenerateCaptchaText();
            captchaImage.Source = captchaLog.CreateCaptchaImage();
            captchaAnswer.Text = "";
            captchaAnswer.Focus();
        }

        private void CheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(captchaAnswer.Text))
            {
                MessageBox.Show("Введите символы с картинки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                captchaAnswer.Focus();
                return;
            }

            bool correct = captchaLog.IsCaptchaCorrect(captchaAnswer.Text);
            if (correct)
            {
                MessageBox.Show("Капча пройдена!\nТеперь введите правильный пароль.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                HideCaptcha();
                passwordPB.Focus();
                passwordPB.SelectAll();
            }
            else
            {
                MessageBox.Show("Неверная капча. Попробуйте ещё раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                if (captchaLog.ShouldRegenerateCaptchaAfterWrongAnswer())
                {
                    captchaLog.GenerateCaptchaText();
                    captchaImage.Source = captchaLog.CreateCaptchaImage();
                }

                captchaAnswer.Text = "";
                captchaAnswer.Focus();
            }
        }

        private void ResetAfterSuccess()
        {
            captchaLog.Reset();
            HideCaptcha();
            ClearAndFocusLoginFields();
        }

        private void ClearAndFocusLoginFields()
        {
            userNameTB.Text = "";
            passwordPB.Password = "";
            userNameTB.Focus();
        }

        private void SwitchMode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLoginMode = !isLoginMode;
            UpdateUIForMode();
            ClearAndFocusLoginFields();
            HideCaptcha();
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
