using PR12.Classes;
using PR12.Entities;
using PR12.Game_Management;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для gamePage.xaml
    /// </summary>
    public partial class gamePage : Page
    {
        private Player player;
        private GameLogic game;
        private ChestItem currentItem;

        private List<Enemy> currentEnemies;
        private Dictionary<Enemy, Image> enemyImages = new Dictionary<Enemy, Image>();
        private bool currentIsBoss;

        public gamePage()
        {
            InitializeComponent();

            player = new Player(100, 17, 12);
            game = new GameLogic(player);

            NextTurn();
        }

        private void NextTurn()
        {
            (currentEnemies, currentItem, currentIsBoss) = game.NextTurn();

            heroesImagesPanel.Items.Clear();

            if (currentEnemies != null)
            {
                ShowEnemies();
            }
            else if (currentItem != null)
            {
                ShowChest();
            }

            UpdatePlayerPanel();
        }

        private void ShowEnemies()
        {
            foreach (var enemy in currentEnemies)
            {
                Image img = new Image();
                img.Source = GetEnemyImage(enemy);
                img.Height = currentIsBoss ? 360 : 220;
                img.Margin = new Thickness(5);
                img.RenderTransformOrigin = new Point(0.5, 0.5);

                TransformGroup group = new TransformGroup();
                img.RenderTransform = group;

                heroesImagesPanel.Items.Add(img);
                enemyImages[enemy] = img;

                AnimateAppearance(img);
            }

            redBtn.Visibility = Visibility.Visible;
            greenBtn.Visibility = Visibility.Visible;
            redBtn.Content = "Атака";
            greenBtn.Content = "Защита";

            LogMessage(currentIsBoss ?
                $"\nВНИМАНИЕ!!!\n{currentEnemies[0].enemyName} появился!" :
                $"\nВас атакуют {currentEnemies.Count} враг(а)!");
        }

        private void RemoveDeadEnemies()
        {
            var dead = currentEnemies.Where(e => e.enemyHP <= 0).ToList();

            foreach (var enemy in dead)
            {
                if (enemyImages.TryGetValue(enemy, out Image img))
                {
                    heroesImagesPanel.Items.Remove(img);
                    enemyImages.Remove(enemy);
                }
            }

            currentEnemies.RemoveAll(e => e.enemyHP <= 0);
        }

        private void ShowChest()
        {
            Grid chestGrid = new Grid();

            Image chestImage = new Image();
            chestImage.Source = new BitmapImage(new Uri("/Image/chest.png", UriKind.Relative));
            chestImage.Height = 200;

            chestGrid.Children.Add(chestImage);

            Image itemImage = new Image();
            itemImage.Margin = new Thickness(20, 20, 0, 0);

            switch (currentItem.Type)
            {
                case "сил атаки":
                    itemImage.Source = new BitmapImage(new Uri("/Image/sword.png", UriKind.Relative));
                    itemImage.Height = 200;
                    break;

                case "ед. защиты":
                    itemImage.Source = new BitmapImage(new Uri("/Image/protection.png", UriKind.Relative));
                    itemImage.Height = 155;
                    break;

                case "Зелье":
                    itemImage.Source = new BitmapImage(new Uri("/Image/healthPotion.png", UriKind.Relative));
                    itemImage.Height = 140;
                    break;
            }

            chestGrid.Children.Add(itemImage);

            heroesImagesPanel.Items.Add(chestGrid);

            if (currentItem.Type == "Зелье")
            {
                player.HealthFull();

                redBtn.Visibility = Visibility.Collapsed;
                greenBtn.Content = "Супер!";
                HPTBl.Text = 100.ToString();

                LogMessage($"\nВы нашли зелье и восстановили HP!");
            }
            else
            {
                redBtn.Visibility = Visibility.Visible;
                greenBtn.Visibility = Visibility.Visible;

                redBtn.Content = "Не взять";
                greenBtn.Content = "Взять";

                LogMessage($"\nВы нашли {currentItem.Name} ({currentItem.Value} {currentItem.Type})");
            }
        }

        private void Btns_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (currentEnemies != null)
            {
                ///это работает отлично если урон по 1 врагу
/*                if (btn.Content.ToString() == "Атака")
                {
                    var logs = game.PlayerAttack(currentEnemies);
                    foreach (var log in logs) LogMessage(log);

                    if (currentEnemies.Count > 0)
                    {
                        Enemy target = currentEnemies[0];
                        if (enemyImages.TryGetValue(target, out Image hitImg) && hitImg != null)
                        {
                            AnimateHit(hitImg);
                        }
                    }

                    var enemyLogs = game.EnemyTurn(currentEnemies);
                    foreach (var log in enemyLogs) LogMessage(log);

                    RemoveDeadEnemies();

                    if (currentEnemies.Count == 0)
                    {
                        LogMessage("\nВсе враги побеждены!");
                        NextTurn();
                    }
                }*/

                if (btn.Content.ToString() == "Атака")
                {
                    var logs = game.PlayerAttack(currentEnemies);
                    foreach (var log in logs) LogMessage(log);

                    foreach (var enemy in currentEnemies.ToList())
                    {
                        if (enemyImages.TryGetValue(enemy, out Image img) && img != null)
                        {
                            AnimateHit(img);
                        }
                    }

                    var enemyLogs = game.EnemyTurn(currentEnemies);
                    foreach (var log in enemyLogs) LogMessage(log);

                    RemoveDeadEnemies();

                    if (currentEnemies.Count == 0)
                    {
                        LogMessage("\nВсе враги побеждены!");
                        NextTurn();
                        return;   
                    }
                }

                if (btn.Content.ToString() == "Защита")
                {
                    bool skeletonPresent = false;

                    foreach (var enemy in currentEnemies)
                    {
                        if (enemy is Skelet || enemy is Pestov)
                        {
                            skeletonPresent = true;
                            break;
                        }
                    }

                    if (skeletonPresent)
                    {
                        LogMessage("\nСкелет игнорирует защиту игрока!");

                        var enemyLogs = game.EnemyTurn(currentEnemies);
                        foreach (var log in enemyLogs) LogMessage(log);
                    }
                    else
                    {
                        var logs = game.PlayerDefend(currentEnemies);
                        foreach (var log in logs) LogMessage(log);
                    }

                    RemoveDeadEnemies();

                    if (currentEnemies.Count == 0)
                    {
                        LogMessage("Все враги побеждены!");
                        NextTurn();
                        return;
                    }
                }
            }

            else if (currentItem != null)
            {
                if (btn.Content.ToString() == "Взять" || btn.Content.ToString() == "Супер!")
                {
                    game.TakeItem(currentItem);
                    LogMessage($"Вы взяли {currentItem.Name}");
                }
                else
                {
                    LogMessage($"Вы оставили {currentItem.Name}");
                }

                currentItem = null;
                NextTurn();
            }

            UpdatePlayerPanel();

            if (game.IsPlayerDead())
            {
                NavigationService.Navigate(new endPage());
            }
        }

        private void UpdatePlayerPanel()
        {
            stepTBl.Text = game.turn.ToString();
            HPTBl.Text = player.playerHP.ToString();
            weaponTBl.Text = player.weaponName;
            protectTBl.Text = player.protectionName;
        }

        private void LogMessage(string message)
        {
            logTBl.Text += message + "\n";
            logScrollViewer.ScrollToEnd();
        }

        private BitmapImage GetEnemyImage(Enemy enemy)
        {
            string imagePath;

            switch (enemy.enemyName)
            {
                case "Гоблин":
                    imagePath = "/Image/goblin.png";
                    break;

                case "Скелет":
                    imagePath = "/Image/skelet.png";
                    break;

                case "Маг":
                    imagePath = "/Image/mage.png";
                    break;

                case "ВВГ (босс гоблинов)":
                    imagePath = "/Image/VVG.png";
                    break;

                case "Ковальский (босс скелетов)":
                    imagePath = "/Image/Kovalsky.png";
                    break;

                case "Архимаг С++ (босс магов)":
                    imagePath = "/Image/MaxCpp.png";
                    break;

                case "Пестов С-- (AAAAA)":
                    imagePath = "/Image/Pestov.png";
                    break;

                default:
                    imagePath = "/Image/goblin.png";
                    break;
            }

            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        private void AnimateAppearance(Image img)
        {
            img.Opacity = 0;

            // Появление (плавное)
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(600));
            img.BeginAnimation(UIElement.OpacityProperty, fade);

            // Увеличение от 0.65 до 1
            var scaleAnim = new DoubleAnimation(0.65, 1.0, TimeSpan.FromMilliseconds(600));

            var scale = new ScaleTransform(0.65, 0.65);
            ((TransformGroup)img.RenderTransform).Children.Add(scale);

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void AnimateHit(Image img)
        {
            if (img == null) return;

            var group = (TransformGroup)img.RenderTransform;

            // Находим или добавляем translate
            var trans = group.Children.OfType<TranslateTransform>().FirstOrDefault();
            if (trans == null)
            {
                trans = new TranslateTransform();
                group.Children.Add(trans);
            }

            // Самая простая встряска
            var shake = new DoubleAnimation
            {
                From = 0,
                To = -20,
                Duration = new Duration(TimeSpan.FromMilliseconds(50)),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(4)   // 4 раза туда-сюда ≈ 0.4 сек
            };

            trans.BeginAnimation(TranslateTransform.XProperty, shake);
        }
    }
}
