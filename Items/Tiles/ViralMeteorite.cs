using JoJoStands.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Items.Tiles
{
    public class ViralMeteorite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Meteorite");
            Tooltip.SetDefault("A concentrated cluster of a foreign virus...");
            SacrificeTotal = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 8);
            Item.createTile = ModContent.TileType<ViralMeteoriteTile>();
        }
    }
}