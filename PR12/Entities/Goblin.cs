using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR12.Game_Management;

namespace PR12.Classes
{
    internal class Goblin : Enemy
    {
        public double critChance;

        public Goblin(string name, double hp, double attack, double protect, double critChance)
            : base(name, hp, attack, protect)
        {
            this.critChance = critChance;
        }

        public override double DamageToPlayer(Player player)
        {
            double damage = enemyAttack - player.playerProtect;

            if (damage < 0)
                damage = 0;

            if (Rand.Critical(critChance))
            {
                damage *= 2;
            }

            return damage;
        }
    }
}
