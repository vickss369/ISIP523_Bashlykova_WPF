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
        private int failedLoginAttempts = 0;
        private string lastFailedLogin = null;
        private string captchaText = "";

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
                    ClearAndFocusLoginFields();
                    return;
                }

                if (user.Password == pass)
                {
                    Users.CurrentLogin = login;
                    Users.CurrentPassword = pass;
                    MessageBox.Show("Вы вошли!");
                    NavigationService.Navigate(new filmsPage());
                    ResetAfterSuccess();
                }
                else
                {
                    failedLoginAttempts++;
                    lastFailedLogin = login;

                    if (failedLoginAttempts >= 3)
                    {
                        passwordPB.Password = "";
                        ShowCaptcha();
                        MessageBox.Show("Неверный пароль.\nПройдите проверку.", "Внимание");
                    }
                    else
                    {
                        MessageBox.Show($"Неверный пароль. Осталось попыток: {3 - failedLoginAttempts}", "Ошибка");
                        passwordPB.Focus();
                    }
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

                passwordPB.Password = "";
            }
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ" + 
                "abcdefghjkmnpqrstuvwxyz" + 
                "23456789" + 
                "!@#$%^&*";

            Random rnd = new Random();
            int length = rnd.Next(6, 8);
            captchaText = new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());

            int width = 240;
            int height = 80;

            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                // фон
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(248, 240, 232)), null, new Rect(0, 0, width, height));

                // мягкие пятна фона
                for (int i = 0; i < 3; i++)
                {
                    var spotBrush = new SolidColorBrush(
                        Color.FromArgb(150, (byte)rnd.Next(180, 220), (byte)rnd.Next(180, 220),(byte)rnd.Next(180, 220)));

                    dc.DrawEllipse(
                        spotBrush, null, new Point(rnd.Next(20, 220), rnd.Next(10, 70)), rnd.Next(25, 40), rnd.Next(20, 35));
                }

                // случайные линии
                for (int i = 0; i < rnd.Next(8, 14); i++)
                {
                    var pen = new Pen(
                        new SolidColorBrush(
                            Color.FromArgb((byte)rnd.Next(130, 190), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150))),
                        rnd.Next(1, 3));

                    Point p1 = new Point(rnd.Next(width), rnd.Next(height));
                    Point p2 = new Point(rnd.Next(width), rnd.Next(height));

                    dc.DrawLine(pen, p1, p2);
                }

                // текст капчи
                double x = width / 2 - (captchaText.Length * 21 / 2);

                foreach (char c in captchaText)
                {
                    byte r = (byte)rnd.Next(80, 180);
                    byte g = (byte)rnd.Next(70, 170);
                    byte b = (byte)rnd.Next(90, 190);

                    var brush = new SolidColorBrush(
                        Color.FromArgb(250, r, g, b)); // яркие буквы

                    var format = new FormattedText(
                        c.ToString(), System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), rnd.Next(28, 36), brush, 96);

                    double y = rnd.Next(10, 30);

                    dc.PushTransform(
                        new RotateTransform(rnd.Next(-30, 30), x + 10, 40));

                    dc.DrawText(format, new Point(x + rnd.Next(-3, 4), y));
                    dc.Pop();

                    x += 22;
                }

                // волнистая линия поверх текста
                for (int i = 0; i < 2; i++)
                {
                    var pen = new Pen(
                        new SolidColorBrush(
                            Color.FromArgb(180, (byte)rnd.Next(80, 120), (byte)rnd.Next(80, 120), (byte)rnd.Next(80, 120))),
                        2);

                    Point prev = new Point(0, rnd.Next(25, 55));
                    for (int x2 = 20; x2 < width; x2 += 20)
                    {
                        Point next = new Point(x2, prev.Y + rnd.Next(-10, 10));
                        dc.DrawLine(pen, prev, next);
                        prev = next;
                    }
                }

                // точки шума
                for (int i = 0; i < rnd.Next(40, 70); i++)
                {
                    double px = rnd.NextDouble() * width;
                    double py = rnd.NextDouble() * height;
                    double size = rnd.NextDouble() * 1.5 + 0.5;

                    var brush = new SolidColorBrush(
                        Color.FromArgb((byte)rnd.Next(80, 130), (byte)rnd.Next(0, 80), (byte)rnd.Next(0, 80), (byte)rnd.Next(0, 80)));

                    dc.DrawEllipse(brush, null, new Point(px, py), size, size);
                }
            }

            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(visual);
            captchaImage.Source = ToBitmapImage(bmp);
        }

        private BitmapImage ToBitmapImage(RenderTargetBitmap bmp)
        {
            var stream = new MemoryStream();
            new PngBitmapEncoder { Frames = { BitmapFrame.Create(bmp) } }.Save(stream);
            stream.Position = 0;

            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
            captchaAnswer.Text = "";
            captchaAnswer.Focus();
        }

        private void CheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(captchaAnswer.Text))
            {
                MessageBox.Show("Введите символы с картинки!", "Ошибка");
                captchaAnswer.Focus();
                return;
            }

            if (captchaAnswer.Text.Trim().Equals(captchaText, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Капча пройдена!\nТеперь введите правильный пароль.", "Успех");
                HideCaptcha();
                passwordPB.Focus();
                passwordPB.SelectAll();
            }
            else
            {
                MessageBox.Show("Неверная капча. Попробуйте ещё раз.", "Ошибка");
                GenerateCaptcha();
                captchaAnswer.Text = "";
                captchaAnswer.Focus();
            }
        }

        private void ShowCaptcha()
        {
            captchaContainer.Visibility = Visibility.Visible;
            entrBtn.Visibility = Visibility.Collapsed;
            switchModeText.Visibility = Visibility.Collapsed;
            GenerateCaptcha();
            captchaAnswer.Focus();
        }

        private void HideCaptcha()
        {
            captchaContainer.Visibility = Visibility.Collapsed;
            entrBtn.Visibility = Visibility.Visible;
            switchModeText.Visibility = Visibility.Visible;
            captchaAnswer.Text = "";
        }

        private void ClearAndFocusLoginFields()
        {
            userNameTB.Text = "";
            passwordPB.Password = "";
            userNameTB.Focus();
        }

        private void ResetAfterSuccess()
        {
            failedLoginAttempts = 0;
            lastFailedLogin = null;
            HideCaptcha();
            ClearAndFocusLoginFields();
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
