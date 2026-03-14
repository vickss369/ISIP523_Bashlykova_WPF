using PR12.Classes;
using PR12.Game_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Entities
{
    internal class Chest
    {
        public static ChestItem GetRandomItem()
        {
            int ch = Rand.GetRandomChoice(1, 6);
            switch (ch)
            {
                case 1: return new ChestItem("Лечебное зелье", "Зелье", 100);
                case 2: return new ChestItem("Деревянный меч", "сил атаки", 20);
                case 3: return new ChestItem("Металлический меч", "сил атаки", 30);
                case 4: return new ChestItem("Деревянная броня", "ед. защиты", 20);
                case 5: return new ChestItem("Железная броня", "ед. защиты", 30);
                default: return new ChestItem("Лечебное зелье", "Зелье", 100);
            }
        }

        public static void TakeItem(Player player, ChestItem item)
        {
            switch (item.Type)
            {
                case "сил атаки":
                    player.weaponName = item.Name;
                    player.playerAttack = item.Value;
                    break;

                case "ед. защиты":
                    player.protectionName = item.Name;
                    player.playerProtect = item.Value;
                    break;
            }
        }
    }
}
