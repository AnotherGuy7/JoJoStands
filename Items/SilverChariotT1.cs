using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SilverChariotT1 : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "SilverChariot";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Chariot (Tier 1)");
            Tooltip.SetDefault("Left-click to stab enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
