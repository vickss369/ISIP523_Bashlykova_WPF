using PR12.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Game_Management
{
    internal class Factory
    {
        private static Goblin CreateRandomGoblin()
        {
            double hp = Rand.GetRandomChoice(20, 32);
            double attack = Rand.GetRandomChoice(8, 15);
            double protect = Rand.GetRandomChoice(2, 5);
            double chanceKritYron = Rand.GetRandomChoice(18, 23);

            return new Goblin("Гоблин", hp, attack, protect, chanceKritYron);
        }

        private static Skelet CreateRandomSkelet()
        {
            double hp = Rand.GetRandomChoice(35, 42);
            double attack = Rand.GetRandomChoice(9, 12);
            double protect = Rand.GetRandomChoice(4, 7);

            return new Skelet("Скелет", hp, attack, protect);
        }

        private static Mag CreateRandomMag()
        {
            double hp = Rand.GetRandomChoice(20, 28);
            double attack = Rand.GetRandomChoice(12, 16);
            double protect = Rand.GetRandomChoice(1, 4);
            double chanceMoroz = Rand.GetRandomChoice(12, 17);

            return new Mag("Маг", hp, attack, protect, chanceMoroz);
        }

        public static Enemy GenerateEnemy()
        {
            int numEnemy = Rand.GetRandomChoice(0, 3);
            switch (numEnemy)
            {
                case 0: return CreateRandomGoblin();
                case 1: return CreateRandomSkelet();
                case 2: return CreateRandomMag();
                default: return CreateRandomGoblin();
            }
        }

        public static Enemy GenerateBoss()
        {
            int numBoss = Rand.GetRandomChoice(0, 4);
            switch (numBoss)
            {
                case 0: return new Goblin("ВВГ (босс гоблинов)", Rand.GetRandomChoice(27, 33) * 2, Rand.GetRandomChoice(17, 22) * 1.5, Rand.GetRandomChoice(10, 19) * 1.2, Rand.GetRandomChoice(26, 31));
                case 1: return new Skelet("Ковальский (босс скелетов)", Rand.GetRandomChoice(37, 42) * 2.2, Rand.GetRandomChoice(15, 24) * 1.3, Rand.GetRandomChoice(13, 21) * 1.4);
                case 2:return new Mag("Архимаг С++ (босс магов)", Rand.GetRandomChoice(25, 31) * 1.8, Rand.GetRandomChoice(17, 29) * 1.6, Rand.GetRandomChoice(10, 18) * 1.2, Rand.GetRandomChoice(20, 26));
                case 3: return new Pestov();
                default: return null;
            }
        }

        public static List<Enemy> GenerateEnemies()
        {
            int count = Rand.GetRandomChoice(1, 4);
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < count; i++)
            {
                enemies.Add(GenerateEnemy());
            }

            return enemies;
        }
    }
}
