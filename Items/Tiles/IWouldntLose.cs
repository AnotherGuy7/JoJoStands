using JoJoStands.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class IWouldntLose : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("I Wouldn't Lose");
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
            Item.createTile = ModContent.TileType<IWouldntLoseTile>();
        }
    }
}