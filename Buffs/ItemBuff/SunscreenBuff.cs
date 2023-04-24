using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SunscreenBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sunscreen");
            // Description.SetDefault("98% of those harmful UV rays are being blocked away!");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.sunburnRegenTimeMultiplier += 3f;       //About 180 regen time
            vPlayer.sunburnDamageMultiplier *= 0.6f;        //About 40% less damage every hit
            vPlayer.sunburnMoveSpeedMultiplier += 0.5f;     //Negates the movement speed reduction effect
        }
    }
}