using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SunscreenBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sunscreen");
            Description.SetDefault("98% of those harmful UV rays are being blocked away!");
            canBeCleared = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.weakenedSunBurning = true;
        }
    }
}