using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class KnifeAmalgamation : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knife Amalgamation");
            Description.SetDefault("You have inserted dozens of knives into yourself!");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}