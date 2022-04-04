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
            vPlayer.sunburnRegenTimeMultiplier += 3f;       //About 180 regen time
            vPlayer.sunburnDamageMultiplier *= 0.6f;        //About 40% less damage every hit
            vPlayer.sunburnMoveSpeedMultiplier += 0.5f;     //Negates the movement speed reduction effect
        }
    }
}