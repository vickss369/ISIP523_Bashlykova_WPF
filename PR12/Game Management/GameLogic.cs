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
                Enemy boss = Factory.GenerateBoss();
                enemies = new List<Enemy> { boss };
                isBoss = true;
            }
            else
            {
                if (Rand.Chance(0.5))
                {
                    enemies = Factory.GenerateEnemies();
                }
                else
                {
                    item = Chest.GetRandomItem();
                }
            }

            return (enemies, item, isBoss);
        }

        public double PlayerAttack(Enemy enemy)
        {
            double dmg = Player.AttackEnemy();
            enemy.enemyHP -= dmg;
            if (enemy.enemyHP < 0) enemy.enemyHP = 0;
            return dmg;
        }

        public double PlayerDefend(double incomingDamage)
        {
            double damage = Player.Defend(incomingDamage);
            Player.playerHP -= damage;
            if (Player.playerHP < 0) Player.playerHP = 0;
            return damage;
        }

        /* public double EnemyAttack(Enemy enemy)
         {
             double dmg = enemy.AttackValue(Player);
             Player.playerHP -= dmg;
             if (Player.playerHP < 0) Player.playerHP = 0;
             return dmg;
         }*/

        public bool IsPlayerDead()
        {
            return Player.playerHP <= 0;
        }
    }
}
