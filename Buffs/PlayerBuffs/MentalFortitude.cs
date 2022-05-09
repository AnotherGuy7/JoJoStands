using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class MentalFortitude : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mental Fortitude");
            Description.SetDefault("Your will is unbreakable.\nDefense increased by 3.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 3;
        }
    }
}