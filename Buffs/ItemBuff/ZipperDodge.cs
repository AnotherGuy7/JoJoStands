using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class ZipperDodge : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zipper Dodge");
            // Description.SetDefault("You predict the movements of your enemies and zip away at yourself to dodge!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.buffTime[buffIndex] = 2;
        }
    }
}