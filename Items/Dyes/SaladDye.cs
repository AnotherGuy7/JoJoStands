using Terraria;
using Terraria.ID;

namespace JoJoStands.Items.Dyes
{
    public class SaladDye : StandDye
    {
        public override string DyePath => "/Salad";

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 25);
        }

        public override void UpdateEquippedDye(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.usingStandTextureDye = true;
            mPlayer.currentTextureDye = MyPlayer.StandTextureDye.Salad;
        }
    }
}
