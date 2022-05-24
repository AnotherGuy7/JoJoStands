using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class ZipperDodge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zipper Dodge");
            Description.SetDefault("You predict the movements of your enemies and zip away at yourself to dodge!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<MyPlayer>().standName.Contains("StickyFingers") && !Main.mouseRight)
                player.buffTime[buffIndex] = 2;
        }
    }
}