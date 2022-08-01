using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT1 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;
        public override string standProjectileName => "KillerQueen";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb Tier 1)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 10 blocks\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
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
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}