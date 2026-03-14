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
            int hp = Rand.GetRandomChoice(22, 32);   
            int attack = Rand.GetRandomChoice(11, 16);  
            int protect = Rand.GetRandomChoice(3, 7);  
            int chanceKritYron = Rand.GetRandomChoice(18, 23);
            return new Goblin("Гоблин", hp, attack, protect, chanceKritYron);
        }

        private static Skelet CreateRandomSkelet()
        {
            int hp = Rand.GetRandomChoice(38, 46);  
            int attack = Rand.GetRandomChoice(11, 15);     
            int protect = Rand.GetRandomChoice(4, 9);
            return new Skelet("Скелет", hp, attack, protect);
        }

        private static Mag CreateRandomMag()
        {
            int hp = Rand.GetRandomChoice(21, 29); 
            int attack = Rand.GetRandomChoice(13, 19);  
            int protect = Rand.GetRandomChoice(1, 7);
            int chanceMoroz = Rand.GetRandomChoice(12, 20);
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
                case 0: return new Goblin("ВВГ (босс гоблинов)", Rand.GetRandomChoice(27, 35) * 2, Rand.GetRandomChoice(19, 26) * 1.5, Rand.GetRandomChoice(15, 20) * 1.2, Rand.GetRandomChoice(26, 31));
                case 1: return new Skelet("Ковальский (босс скелетов)", Rand.GetRandomChoice(37, 42) * 2.2, Rand.GetRandomChoice(19, 28) * 1.3, Rand.GetRandomChoice(15, 23) * 1.4);
                case 2:return new Mag("Архимаг С++ (босс магов)", Rand.GetRandomChoice(25, 33) * 1.8, Rand.GetRandomChoice(20, 29) * 1.6, Rand.GetRandomChoice(13, 18) * 1.2, Rand.GetRandomChoice(20, 26));
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
