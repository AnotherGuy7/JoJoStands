using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedT1 : StandItemClass
    {
        public override int standSpeed => 20;
        public override int standType => 2;
        public override string standProjectileName => "MagiciansRed";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Tier 1)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
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
