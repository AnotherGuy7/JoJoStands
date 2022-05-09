using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Tiles
{
    public class ViralMeteorite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Meteorite");
            Tooltip.SetDefault("A concentrated cluster of a foreign virus...");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 45, 0);
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<ViralMeteoriteTile");
        }
    }
}