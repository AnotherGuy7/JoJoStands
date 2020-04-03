using Terraria;
using Terraria.ModLoader;

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
            item.width = 32;
            item.height = 36;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 1;
            item.createTile = mod.TileType("BloodForTheKingTile");
        }
    }
}