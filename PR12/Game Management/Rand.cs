using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Game_Management
{
    internal class Rand
    {
        private static readonly Random rand = new Random();

        public static int GetRandomChoice(int start, int end)
        {
            return rand.Next(start, end);
        }

        public static bool Chance(double chance)
        {
            return rand.NextDouble() < chance;
        }

        public static bool Evade(double chance = 0.4)
        {
            return Chance(chance);
        }


        public static bool Critical(double chance = 0.3)
        {
            return Chance(chance);
        }

        public static bool Freeze(double chance = 0.3)
        {
            return Chance(chance);
        }

        public static int BlockPercent(int min = 70, int max = 100)
        {
            return GetRandomChoice(min, max + 1);
        }
    }
}
