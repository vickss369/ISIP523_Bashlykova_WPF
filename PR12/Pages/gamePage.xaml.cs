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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для gamePage.xaml
    /// </summary>
    public partial class gamePage : Page
    {
        private Player player;
        private GameLogic game;
        private List<Enemy> currentEnemies;
        private ChestItem currentItem;
        private bool currentIsBoss;

        public gamePage()
        {
            InitializeComponent();

            player = new Player(100, 15, 10);
            game = new GameLogic(player);
            NextTurn();
        }

        private void NextTurn()
        {
            (currentEnemies, currentItem, currentIsBoss) = game.NextTurn();

            heroesImagesPanel.Items.Clear();
            if (currentEnemies != null && currentEnemies.Count > 0)
            {
                foreach (var enemy in currentEnemies)
                {
                    heroesImagesPanel.Items.Add(GetEnemyImage(enemy));
                }

                redBtn.Visibility = Visibility.Visible;
                greenBtn.Visibility = Visibility.Visible;

                redBtn.Content = "Атака";
                greenBtn.Content = "Защита";

                LogMessage(currentIsBoss ? $"Босс {currentEnemies[0].enemyName} появился!" :
                                           $"Вас атакуют {currentEnemies.Count} врага(ов)!");
            }
            else if (currentItem != null)
            {
                heroesImagesPanel.Items.Clear();
                Grid chestGrid = new Grid();

                Image chestImage = new Image();
                chestImage.Source = new BitmapImage(new Uri("/Image/chest.png", UriKind.Relative));
                chestImage.Height = 220;
                chestImage.Stretch = Stretch.Uniform;
                chestGrid.Children.Add(chestImage);

                Image itemImage = new Image();
                itemImage.Margin = new Thickness(30, 25, 0, 0);
                itemImage.VerticalAlignment = VerticalAlignment.Center;
                itemImage.HorizontalAlignment = HorizontalAlignment.Center;

                switch (currentItem.Type)
                {
                    case "к атаке":
                        itemImage.Source = new BitmapImage(new Uri("/Image/sword.png", UriKind.Relative));
                        itemImage.Height = 200;
                        break;

                    case "к защите":
                        itemImage.Source = new BitmapImage(new Uri("/Image/protection.png", UriKind.Relative));
                        itemImage.Height = 170;
                        break;

                    case "Potion":
                        itemImage.Source = new BitmapImage(new Uri("/Image/healthPotion.png", UriKind.Relative));
                        itemImage.Height = 140;
                        break;
                }
                chestGrid.Children.Add(itemImage);
                heroesImagesPanel.Items.Add(chestGrid);

                if (currentItem.Type == "Potion")
                {
                    redBtn.Visibility = Visibility.Collapsed;
                    greenBtn.Content = "Супер!";

                    player.HealthFull();
                    UpdatePlayerPanel();

                    LogMessage($"Вы нашли зелье и восстановили здоровье до {player.playerHP} HP!");
                }
                else
                {
                    redBtn.Visibility = Visibility.Visible;
                    greenBtn.Visibility = Visibility.Visible;

                    redBtn.Content = "Не взять";
                    greenBtn.Content = "Взять";

                    LogMessage($"Вы нашли {currentItem.Name} (+{currentItem.Value} {currentItem.Type})!");
                }
            }

            UpdatePlayerPanel();
        }

        private void Btns_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (currentEnemies != null)
            {
                if (btn.Content.ToString() == "Атака")
                {
                    foreach (var enemy in currentEnemies)
                    {
                        double dmg = player.AttackEnemy();
                        enemy.enemyHP -= dmg;
                        LogMessage($"Вы нанесли {dmg} ед. урона {enemy.enemyName}!");

                        double enemyDmg = enemy.DamageToPlayer(player);
                        player.playerHP -= enemyDmg;
                        LogMessage($"{enemy.enemyName} нанес вам {enemyDmg} ед. урона!");

                        if (enemy is Mag magEnemy && Rand.Freeze(magEnemy.freezeChance))
                        {
                            player.isFrozen = true;
                            LogMessage("Вы заморожены магией!");
                        }
                        else if (enemy is Pestov pestovEnemy && Rand.Freeze(pestovEnemy.freezeChance))
                        {
                            player.isFrozen = true;
                            LogMessage("Пестов использовал свою способность! Вы заморожены!");
                        }
                    }
                }
                else if (btn.Content.ToString() == "Защита")
                {
                    if (player.TryEvade())
                    {
                        LogMessage("Вы успешно уклонились от следующей атаки!");
                    }
                    else
                    {
                        LogMessage("Уклон не удался. Активирован блок.");
                        foreach (var enemy in currentEnemies)
                        {
                            double dmg = enemy.DamageToPlayer(player);
                            dmg = player.Defend(dmg);
                            player.playerHP -= dmg;
                            LogMessage($"{enemy.enemyName} нанес {dmg} ед. после блока.");
                        }
                    }
                }

                currentEnemies.RemoveAll(en => en.enemyHP <= 0);

                if (currentEnemies.Count == 0)
                {
                    LogMessage("Все враги повержены!");
                    NextTurn();
                }
            }
            else if (currentItem != null)
            {
                if (btn.Content.ToString() == "Взять")
                {
                    Chest.TakeItem(player, currentItem);
                    LogMessage($"Вы экипировали {currentItem.Name}!");
                }
                else if (btn.Content.ToString() == "Не взять")
                {
                    LogMessage($"Вы пропустили {currentItem.Name}.");
                }

                currentItem = null;
                NextTurn();
            }
            UpdatePlayerPanel();

            if (player.playerHP <= 0)
            {
                LogMessage("Вы погибли!");
                NavigationService.Navigate(new endPage());
            }
        }

        private void UpdatePlayerPanel()
        {
            stepTBl.Text = game.turn.ToString();
            HPTBl.Text = player.playerHP.ToString();
            weaponTBl.Text = $"{player.weaponName}";
            protectTBl.Text = $"{player.protectionName}";
        }

        private void LogMessage(string message)
        {
            logTBl.Text += message + "\n";
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
                default:
                    imagePath = "/Image/goblin.png";
                    break;
            }

            return new BitmapImage(new System.Uri(imagePath, System.UriKind.Relative));
        }
    }
}
