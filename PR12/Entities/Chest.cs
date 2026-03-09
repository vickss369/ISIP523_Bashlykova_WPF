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
                case 1: return new ChestItem("Лечебное зелье", "Potion", 100);
                case 2: return new ChestItem("Деревянный меч", "Weapon", 20);
                case 3: return new ChestItem("Металлический меч", "Weapon", 30);
                case 4: return new ChestItem("Деревянная броня", "Armor", 20);
                case 5: return new ChestItem("Железная броня", "Armor", 30);
                default: return new ChestItem("Лечебное зелье", "Potion", 100);
            }
        }

        public static void TakeItem(Player player, ChestItem item)
        {
            switch (item.Type)
            {
                /*case "Potion":
                    player.HealthFull();
                    break;*/

                case "Weapon":
                    player.weaponName = item.Name;
                    player.playerAttack = item.Value;
                    break;

                case "Armor":
                    player.protectionName = item.Name;
                    player.playerProtect = item.Value;
                    break;
            }
        }
    }
}
