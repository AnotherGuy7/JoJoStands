using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HamonWeaponImbueBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Weapon Imbue");
            Description.SetDefault("You're injecting Hamon into your weapons.");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}