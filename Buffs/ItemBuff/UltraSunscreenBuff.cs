using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class UltraSunscreenBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultra Sunscreen");
            Description.SetDefault("The sunglight can't even touch you!");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<VampirePlayer>().noSunBurning = true;
        }
    }
}