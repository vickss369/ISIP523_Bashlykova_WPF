using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR12.Game_Management;

namespace PR12.Classes
{
    internal class Player
    {
        public double maxHP;
        public double playerHP;
        public double playerAttack;
        public double playerProtect;

        public bool isFrozen;

        public string weaponName;
        public string protectionName;

        public Player(double hp, double attack, double protect)
        {
            maxHP = hp;
            playerHP = hp;

            playerAttack = attack;
            playerProtect = protect;

            weaponName = "Кулаки";
            protectionName = "Одежда";

            isFrozen = false;
        }

        public double AttackEnemy()
        {
            if (isFrozen)
            {
                isFrozen = false;
                return 0;
            }

            return playerAttack;
        }

        public bool TryEvade()
        {
            return Rand.Evade();
        }

        public double Defend(double incomingDamage)
        {
            if (Rand.Evade()) return 0;

            int blockPercent = Rand.BlockPercent();
            double blockValue = playerProtect * (blockPercent / 100.0);

            double damage = incomingDamage - blockValue;
            if (damage < 0) damage = 0;
            return damage;
        }


        /*public void TakeDamage(double damage)
        {
            playerHP -= damage;

            if (playerHP < 0)
                playerHP = 0;
        }*/

        public void HealthFull()
        {
            playerHP = maxHP;
        }
    }
}
