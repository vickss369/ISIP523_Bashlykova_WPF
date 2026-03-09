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
        /*private static Goblin CreateGoblin()
        {
            return new Goblin("Гоблин", 30, 12, 3, 0.20);
        }

        private static Skelet CreateSkelet()
        {
            return new Skelet(
                "Скелет",
                40,
                10,
                5);
        }

        private static Mag CreateMag()
        {
            return new Mag(
                "Маг",
                25,
                15,
                2,
                0.15);
        }*/

        private static Goblin CreateRandomGoblin()
        {
            double hp = Rand.GetRandomChoice(20, 31);
            double attack = Rand.GetRandomChoice(10, 21);
            double protect = Rand.GetRandomChoice(10, 15);
            double chanceKritYron = Rand.GetRandomChoice(15, 26);

            return new Goblin("Гоблин", hp, attack, protect, chanceKritYron);
        }

        private static Skelet CreateRandomSkelet()
        {
            double hp = Rand.GetRandomChoice(30, 41);
            double attack = Rand.GetRandomChoice(15, 15);
            double protect = Rand.GetRandomChoice(10, 16);

            return new Skelet("Скелет", hp, attack, protect);
        }

        private static Mag CreateRandomMag()
        {
            double hp = Rand.GetRandomChoice(40, 46);
            double attack = Rand.GetRandomChoice(20, 31);
            double protect = Rand.GetRandomChoice(13, 15);
            double chanceMoroz = Rand.GetRandomChoice(25, 36);

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

        /*public static Enemy GenerateBoss()
        {
            int type = Rand.GetRandomChoice(0, 4);

            switch (type)
            {
                case 0:
                    return new Goblin(
                        "ВВГ",
                        30 * 2.0,
                        12 * 1.5,
                        3 * 1.2,
                        0.30);

                case 1:
                    return new Skelet(
                        "Ковальский",
                        40 * 2.5,
                        10 * 1.3,
                        5 * 1.4);

                case 2:
                    return new Mag(
                        "Архимаг C++",
                        25 * 1.8,
                        15 * 1.6,
                        2 * 1.1,
                        0.25);

                case 3:
                    return new Pestov();

                default:
                    return new Pestov();
            }
        }*/

        public static Enemy GenerateBoss()
        {
            int numBoss = Rand.GetRandomChoice(0, 4);
            switch (numBoss)
            {
                case 0: return new Goblin("ВВГ (босс гоблинов)", Rand.GetRandomChoice(20, 26) * 2, Rand.GetRandomChoice(10, 16) * 1.5, Rand.GetRandomChoice(10, 16) * 1.2, Rand.GetRandomChoice(15, 26));
                case 1: return new Skelet("Ковальский (босс скелетов)", Rand.GetRandomChoice(30, 36) * 2.5, Rand.GetRandomChoice(15, 21) * 1.3, Rand.GetRandomChoice(10, 16) * 1.4);
                case 2: return new Mag("Архимаг С++ (босс магов)", Rand.GetRandomChoice(40, 46) * 1.8, Rand.GetRandomChoice(20, 26) * 1.6, Rand.GetRandomChoice(10, 16) * 1.1, Rand.GetRandomChoice(25, 36));
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
