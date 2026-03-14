using PR12.Classes;
using PR12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Game_Management
{
    internal class GameLogic
    {
        public Player Player;
        public int turn = 0;

        public GameLogic(Player player)
        {
            Player = player;
        }

        public (List<Enemy> enemies, ChestItem item, bool isBoss) NextTurn()
        {
            turn++;

            List<Enemy> enemies = null;
            ChestItem item = null;
            bool isBoss = false;

            if (turn % 10 == 0)
            {
                enemies = new List<Enemy>();
                enemies.Add(Factory.GenerateBoss());
                isBoss = true;
            }
            else
            {
                int eventType = Rand.Event50();
                if (eventType == 0)
                {
                    item = Chest.GetRandomItem();
                }
                else
                {
                    enemies = Factory.GenerateEnemies();
                }
            }

            return (enemies, item, isBoss);
        }

        public List<string> PlayerAttack(List<Enemy> enemies)
        {
            List<string> logs = new List<string>();

            if (enemies == null || enemies.Count == 0) return logs;

            Enemy target = enemies[0];

            double playerAttack = Player.playerAttack;
            double enemyDefense = target.enemyProtect;

            double damage = playerAttack - enemyDefense;
            if (damage < 0) damage = 0;

            target.enemyHP -= damage;
            if (target.enemyHP < 0) target.enemyHP = 0;

            logs.Add($"\nВы нанесли {damage} урона {target.enemyName}");
            logs.Add($"HP врага = {target.enemyHP}");

            if (target.enemyHP == 0)
            {
                logs.Add($"{target.enemyName} повержен!");
            }

            return logs;
        }

        public List<string> EnemyTurn(List<Enemy> enemies)
        {
            List<string> logs = new List<string>();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.enemyHP <= 0) continue;

                double baseAttack = enemy.enemyAttack;
                double damage = baseAttack;

                if (enemy.enemyName == "Гоблин")
                {
                    if (Rand.Critical(0.2))
                    {
                        damage *= 2;
                        logs.Add("\nКритический удар!");
                    }
                }

                if (enemy.enemyName == "Скелет")
                {
                    //logs.Add("Скелет игнорирует защиту!");
                }
                else
                {
                    double playerProtect = Player.playerProtect;

                    damage -= playerProtect;
                    if (damage < 0)damage = 0;
                }

                Player.playerHP -= damage;
                if (Player.playerHP < 0) Player.playerHP = 0;

                logs.Add($"\n{enemy.enemyName} нанес {damage} урона");

                if (enemy.enemyName == "Маг")
                {
                    if (Rand.Freeze(0.15))
                    {
                        Player.isFrozen = true;
                        logs.Add("Маг заморозил игрока!");
                    }
                }
            }

            return logs;
        }

        public List<string> PlayerDefend(List<Enemy> enemies)
        {
            List<string> logs = new List<string>();

            foreach (Enemy enemy in enemies)
            {
                double baseDamage = enemy.enemyAttack;

                if (Rand.Evade())
                {
                    logs.Add($"Вы уклонились от {enemy.enemyName}");
                    continue;
                }

                int blockPercent = Rand.BlockPercent();
                double protect = Player.playerProtect;

                double reduction = protect * blockPercent / 100.0;

                double damage = baseDamage - reduction;
                if (damage < 0) damage = 0;
          
/*                logs.Add($"Атака врага = {baseDamage}");
                logs.Add($"Защита игрока = {protect}");*/
                logs.Add($"Блок = {blockPercent}%");

                /*logs.Add($"Снижение = защита × блок%");
                logs.Add($"{protect} × {blockPercent}/100 = {reduction}");*/

                logs.Add($"Итоговый урон = {baseDamage} - {reduction} = {damage}");

/*                logs.Add("=========================");
                logs.Add("")*/;

                Player.playerHP -= damage;

                if (Player.playerHP < 0)
                    Player.playerHP = 0;

                logs.Add($"{enemy.enemyName} нанес {damage} урона");
                //logs.Add($"HP игрока = {Player.playerHP}");
            }

            return logs;
        }

        public void TakeItem(ChestItem item)
        {
            Chest.TakeItem(Player, item);
        }

        public bool IsPlayerDead()
        {
            return Player.playerHP <= 0;
        }
    }
}
