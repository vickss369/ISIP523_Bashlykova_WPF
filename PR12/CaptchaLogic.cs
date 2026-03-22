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
    internal class CaptchaLogic
    {
        public string captchaText { get; private set; } = "";
        public int failedLoginAttempts { get; private set; } = 0;
        public string lastFailedLogin { get; private set; } = null;

        private readonly Random rnd = new Random();

        public void Reset()
        {
            failedLoginAttempts = 0;
            lastFailedLogin = null;
            captchaText = "";
        }

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
                           "23456789" +
                           "!@#$%^&*";

            int length = rnd.Next(6, 8);
            captchaText = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)])
                .ToArray());
        }

        public bool IsCaptchaCorrect(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return false;
            }

            return userInput.Trim().Equals(captchaText, StringComparison.OrdinalIgnoreCase);
        }

        public bool ShouldRegenerateCaptchaAfterWrongAnswer()
        {
            return true;
        }

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
                // фон
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(248, 240, 232)), null, new Rect(0, 0, width, height));

                // мягкие пятна фона
                for (int i = 0; i < 3; i++)
                {
                    var spotBrush = new SolidColorBrush(
                        Color.FromArgb(150, (byte)rnd.Next(180, 220), (byte)rnd.Next(180, 220), (byte)rnd.Next(180, 220)));
                    
                    dc.DrawEllipse(
                        spotBrush, null, new Point(rnd.Next(20, 220), rnd.Next(10, 70)), rnd.Next(25, 40), rnd.Next(20, 35));
                }

                // случайные линии
                for (int i = 0; i < rnd.Next(8, 14); i++)
                {
                    var pen = new Pen(
                        new SolidColorBrush(
                            Color.FromArgb((byte)rnd.Next(130, 190), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150), (byte)rnd.Next(60, 150))), rnd.Next(1, 3));
                    
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
                    var brush = new SolidColorBrush(Color.FromArgb(250, r, g, b));  // яркие буквы

                    var format = new FormattedText(
                        c.ToString(), System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), rnd.Next(28, 36), brush, 96);

                    double y = rnd.Next(10, 30);

                    dc.PushTransform(new RotateTransform(rnd.Next(-30, 30), x + 10, 40));
                    dc.DrawText(format, new Point(x + rnd.Next(-3, 4), y));
                    dc.Pop();

                    x += 22;
                }

                // волнистая линия поверх текста
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
