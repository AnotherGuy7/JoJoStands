using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Locked : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Locked");
            Description.SetDefault("Your guilt is increasing and it hurts.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}