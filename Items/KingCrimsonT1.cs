using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonT1 : StandItemClass
    {
        public override int standSpeed => 28;
        public override int standType => 1;
        public override string standProjectileName => "KingCrimson";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Tier 1)");
            Tooltip.SetDefault("Donut enemies with a powerful punch!\nConsecutive Donuts deal greater damage.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
