using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT1 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;
        public override string standProjectileName => "CrazyDiamond";
        public override int standTier => 1;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Tier 1)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate!\nSpecial: Switch to Restoration Mode\nLeft-click in Restoration Mode to perform a restorative punch and right-click to uncraft the item you hold!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
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
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
