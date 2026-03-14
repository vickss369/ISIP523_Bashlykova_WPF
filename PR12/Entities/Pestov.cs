using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR12.Game_Management;

namespace PR12.Classes
{
    internal class Pestov : Skelet
    {
        public double freezeChance;

        public Pestov()
            : base(
                "Пестов С-- (AAAAA)",
                Rand.GetRandomChoice(35, 41) * 1.3,
                Rand.GetRandomChoice(17, 23) * 1.8,
                Rand.GetRandomChoice(15, 20) * 0.6)
        {
            freezeChance = Rand.GetRandomChoice(23, 32) + (Rand.GetRandomChoice(23, 32) / 100 * 15);
        }

        public override double DamageToPlayer(Player player)
        {
            double damage = enemyAttack;

            if (Rand.Freeze(freezeChance))
            {
                player.isFrozen = true;
            }

            return damage;
        }
    }
}
