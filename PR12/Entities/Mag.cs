using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR12.Game_Management;

namespace PR12.Classes
{
    internal class Mag : Enemy
    {
        public double freezeChance;

        public Mag(string name, double hp, double attack, double protect, double freezeChance)
            : base(name, hp, attack, protect)
        {
            this.freezeChance = freezeChance;
        }

        public override double DamageToPlayer(Player player)
        {
            double damage = enemyAttack - player.playerProtect;

            if (damage < 0)
                damage = 0;

            if (Rand.Freeze(freezeChance))
            {
                player.isFrozen = true;
            }

            return damage;
        }
    }
}
