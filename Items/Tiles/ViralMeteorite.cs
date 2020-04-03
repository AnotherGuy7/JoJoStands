using Terraria;
using Terraria.ModLoader;

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
            item.width = 32;
            item.height = 32;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 45, 0);
            item.rare = 1;
            item.maxStack = 999;
            item.createTile = mod.TileType("ViralMeteoriteTile");
        }
    }
}