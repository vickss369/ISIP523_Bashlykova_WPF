using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace PR12
{
    /// <summary>
    /// Класс, реализующий всю логику работы CAPTCHA в приложении: 
    /// управление счётчиком неудачных попыток входа, генерацию текста капчи, 
    /// проверку введённого пользователем ответа и создание искажённого изображения.
    /// </summary>
    public class CaptchaLogic
    {
        public string captchaText { get; set; } = "";
        public int failedLoginAttempts { get; set; } = 0;
        public string lastFailedLogin { get; set; } = null;

        private readonly Random rnd = new Random();

        /// <summary>
        /// Сбрасывает всё состояние капчи в исходное: обнуляет счётчик попыток, последний неудачный логин и текущий текст капчи.
        /// </summary>
        /// <remarks>Вызывается после успешного входа или успешного прохождения капчи.</remarks>
        public void Reset()
        {
            failedLoginAttempts = 0;
            lastFailedLogin = null;
            captchaText = "";
        }

        /// <summary>
        /// Регистрирует неудачную попытку ввода пароля.
        /// </summary>
        /// <param name="attemptedLogin">Логин пользователя, с которым была сделана попытка.</param>
        /// <remarks>Увеличивает счётчик попыток и обновляет поле последнего неудачного логина.</remarks>
        public void RegisterFailedPasswordAttempt(string attemptedLogin)
        {
            failedLoginAttempts++;
            lastFailedLogin = attemptedLogin;
        }

        public bool ShouldShowCaptcha()
        {
            return failedLoginAttempts >= 3;
        }

        public void GenerateCaptchaText()
        {
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ" +
                           "abcdefghjkmnpqrstuvwxyz" +
                           "123456789" +
                           "!@#$%^&*";

            int length = rnd.Next(6, 9);
            captchaText = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)])
                .ToArray());
        }

        /// <summary>
        /// Проверяет, правильно ли пользователь ввёл символы капчи.
        /// </summary>
        /// <param name="userInput">Строка, введённая пользователем в поле ответа.</param>
        /// <returns>true, если введённый текст в точности совпадает с текущим <see cref="captchaText"/> (с учётом регистра), иначе false.</returns>
        /// <remarks>Сравнение строгое (case-sensitive). Пустой или состоящий только из пробелов ввод считается неверным.</remarks>
        public bool IsCaptchaCorrect(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return false;

            return userInput.Trim() == captchaText;
        }

        public bool ShouldRegenerateCaptchaAfterWrongAnswer()
        {
            return true;
        }

        /// <summary>
        /// Создаёт графическое изображение капчи на основе текущего текста <see cref="captchaText"/>.
        /// </summary>
        /// <returns>Объект <see cref="BitmapImage"/>, содержащий готовое искажённое изображение, 
        /// или <c>null</c>, если текст капчи пустой или не задан.</returns>
        /// <remarks>
        /// Изображение размером 240×80 пикселей создаётся с помощью WPF <see cref="DrawingVisual"/> и <see cref="RenderTargetBitmap"/>.  
        /// Применяются следующие элементы защиты от автоматического распознавания:
        /// <list type="bullet">
        ///   <item><description>Светло-бежевый фон с мягкими цветными пятнами (3 эллипса с полупрозрачным градиентом).</description></item>
        ///   <item><description>Случайные линии разной толщины и цвета (8–14 шт.) для создания шума.</description></item>
        ///   <item><description>Текст капчи, отрисованный символ за символом: каждый символ имеет случайный яркий цвет, 
        ///   размер шрифта 28–36 pt, поворот на угол от -30° до +30° и небольшое случайное смещение по X и Y.</description></item>
        ///   <item><description>Две волнистые линии поверх текста (толщина 2 px, лёгкие колебания по высоте).</description></item>
        ///   <item><description>40–70 случайных точек разного размера и тёмно-красного оттенка для дополнительного шума.</description></item>
        /// </list>
        /// После отрисовки визуальный объект преобразуется в PNG-поток и загружается в <see cref="BitmapImage"/>.
        /// </remarks>
        public BitmapImage CreateCaptchaImage()
        {
            if (string.IsNullOrEmpty(captchaText))
            {
                return null;
            }

            int width = 240;
            int height = 80;
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(248, 240, 232)), null, new Rect(0, 0, width, height));

                for (int i = 0; i < 3; i++)
                {
                    var spotBrush = new SolidColorBrush(
                        Color.FromArgb(150, (byte)rnd.Next(180, 220), (byte)rnd.Next(180, 220), (byte)rnd.Next(180, 220)));
                    
                    dc.DrawEllipse(
                        spotBrush, null, new Point(rnd.Next(20, 220), rnd.Next(10, 70)), rnd.Next(25, 40), rnd.Next(20, 35));
                }

                for (int i = 0; i < rnd.Next(8, 14); i++)
                {
                    var pen = new Pen(
                        new SolidColorBrush(
                            Color.FromArgb((byte)rnd.Next(130, 190), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150))), rnd.Next(1, 3));
                    
                    Point p1 = new Point(rnd.Next(width), rnd.Next(height));
                    Point p2 = new Point(rnd.Next(width), rnd.Next(height));
                    
                    dc.DrawLine(pen, p1, p2);
                }

                double x = width / 2 - (captchaText.Length * 21 / 2);
                foreach (char c in captchaText)
                {
                    byte r = (byte)rnd.Next(80, 180);
                    byte g = (byte)rnd.Next(70, 170);
                    byte b = (byte)rnd.Next(90, 190);
                    var brush = new SolidColorBrush(Color.FromArgb(250, r, g, b));  // яркие буквы

                    var format = new FormattedText(
                        c.ToString(), System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), rnd.Next(28, 36), brush, 96);

                    double y = rnd.Next(10, 30);

                    dc.PushTransform(new RotateTransform(rnd.Next(-30, 30), x + 10, 40));
                    dc.DrawText(format, new Point(x + rnd.Next(-3, 4), y));
                    dc.Pop();

                    x += 22;
                }

                for (int i = 0; i < 2; i++)
                {
                    var pen = new Pen(
                        new SolidColorBrush(
                            Color.FromArgb(180, (byte)rnd.Next(80, 120), (byte)rnd.Next(80, 120), (byte)rnd.Next(80, 120))), 2);
                   
                    Point prev = new Point(0, rnd.Next(25, 55));
                    for (int x2 = 20; x2 < width; x2 += 20)
                    {
                        Point next = new Point(x2, prev.Y + rnd.Next(-10, 10));
                        dc.DrawLine(pen, prev, next);
                        prev = next;
                    }
                }

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

            var stream = new System.IO.MemoryStream();
            new PngBitmapEncoder { Frames = { BitmapFrame.Create(bmp) } }.Save(stream);
            stream.Position = 0;

            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();

            return img;
        }
    }
}
