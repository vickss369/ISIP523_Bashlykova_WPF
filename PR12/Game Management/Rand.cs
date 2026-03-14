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

        private static int lastEvent = -1;
        private static int repeatCount = 0;

        private static int lastFreeze = -5;
        private static int turnCounter = 0;

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

        public static bool Critical(double chance = 0.2)
        {
            return Chance(chance);
        }

        public static bool Freeze(double chance = 0.15)
        {
            turnCounter++;

            if (turnCounter - lastFreeze < 2)
                return false;

            bool result = Chance(chance);

            if (result)
                lastFreeze = turnCounter;

            return result;
        }

        public static int BlockPercent(int min = 70, int max = 100)
        {
            return GetRandomChoice(min, max + 1);
        }

        public static int Event50()
        {
            int value = rand.Next(0, 2);

            if (value == lastEvent)
            {
                repeatCount++;

                if (repeatCount >= 2)
                {
                    value = 1 - value;
                    repeatCount = 0;
                }
            }
            else
            {
                repeatCount = 0;
            }

            lastEvent = value;

            return value;
        }
    }
}
