using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Classes
{
    internal class Skelet : Enemy
    {
        public Skelet(string name, double hp, double attack, double protect)
            : base(name, hp, attack, protect)
        {
        }

        public override double DamageToPlayer(Player player)
        {
            return enemyAttack;
        }
    }
}
