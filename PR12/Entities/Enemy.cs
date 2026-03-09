using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Classes
{
    internal class Enemy
    {
        public string enemyName;

        public double enemyHP;
        public double enemyAttack;
        public double enemyProtect;

        public Enemy(string name, double hp, double attack, double protect)
        {
            enemyName = name;
            enemyHP = hp;
            enemyAttack = attack;
            enemyProtect = protect;
        }

        public virtual double DamageToPlayer(Player player)
        {
            double damage = enemyAttack - player.playerProtect;

            if (damage < 0)
                damage = 0;

            return damage;
        }

        public virtual void TakeDamage(double rawDamage)
        {
            double damage = rawDamage - enemyProtect;

            if (damage < 0)
                damage = 0;

            enemyHP -= damage;

            if (enemyHP < 0)
                enemyHP = 0;
        }

        public bool IsAlive()
        {
            return enemyHP > 0;
        }
    }
}
