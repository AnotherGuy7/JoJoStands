using Terraria;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class MentalFortitude : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mental Fortitude");
            Description.SetDefault("Your will is unbreakable.\nDefense increased by 3.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.statDefense += 3;
        }
    }
}