using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class UltraSunscreenBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Sunscreen");
            Description.SetDefault("The sunglight can't even touch you!");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.noSunBurning = true;
        }
    }
}