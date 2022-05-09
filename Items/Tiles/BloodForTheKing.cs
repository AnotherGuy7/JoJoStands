using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Tiles
{
    public class BloodForTheKing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood for the King");
            Tooltip.SetDefault("`D.Storm`");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 1;
            Item.createTile = ModContent.TileType<BloodForTheKingTile");
        }
    }
}