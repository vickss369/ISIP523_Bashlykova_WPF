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
                Rand.GetRandomChoice(30, 41) * 1.3,
                Rand.GetRandomChoice(15, 26) * 1.8,
                Rand.GetRandomChoice(10, 16) * 0.6)
        {
            freezeChance = Rand.GetRandomChoice(25, 36) + (Rand.GetRandomChoice(25, 36) / 100 * 10);
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
