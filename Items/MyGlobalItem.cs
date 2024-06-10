using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using SeldomArchipelago.Systems;

namespace SeldomArchipelago.Items
{
    public class MyGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public static List<int> WeaponIDsToModify { get; private set; }

        private List<int> LoadWeaponIDs()
        {
            var archipelagoSystem = ModContent.GetInstance<ArchipelagoSystem>();
            long weaponClass = archipelagoSystem.getWeaponClass();
            var jsonData = ModContent.GetInstance<SeldomArchipelago>().GetFileBytes("weapons.json");
            var weaponData = JsonConvert.DeserializeObject<WeaponData>(System.Text.Encoding.UTF8.GetString(jsonData));
            WeaponIDsToModify = new List<int>();
            if (weaponClass == (long) 0)
            {
                foreach (var weaponName in weaponData.weaponNames)
                {
                    int itemID = GetItemIDByName(weaponName);
                    if (itemID != 0)
                    {
                        WeaponIDsToModify.Add(itemID);
                    }
                }
            }
            return WeaponIDsToModify;
        }

        public override void SetDefaults(Item item)
        {
            WeaponIDsToModify = LoadWeaponIDs();
            if (WeaponIDsToModify != null && WeaponIDsToModify.Contains(item.type))
            {
                item.damage = 0;
                Main.NewText($"Setting {item.Name} damage to 0", 255, 255, 0);
            }
        }

        private int GetItemIDByName(string itemName)
        {
            int itemID;
            if (ItemID.Search.TryGetId(itemName, out itemID))
            {
                return itemID; // Vanilla item ID found
            }
            else
            {
                return 0; //no item id found
            }
        }
    }

    public class WeaponData
    {
        //public List<int> weaponIDs { get; set; }
        public List<string> weaponNames { get; set; }
    }
}