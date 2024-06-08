using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace SeldomArchipelago.Items
{
    public class MyGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public static List<int> WeaponIDsToModify { get; private set; }

        private List<int> LoadWeaponIDs()
        {
            var jsonData = ModContent.GetInstance<SeldomArchipelago>().GetFileBytes("weapons.json");
            var weaponData = JsonConvert.DeserializeObject<WeaponData>(System.Text.Encoding.UTF8.GetString(jsonData));
            WeaponIDsToModify = weaponData.weaponIDs;
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
    }

    public class WeaponData
    {
        public List<int> weaponIDs { get; set; }
    }
}