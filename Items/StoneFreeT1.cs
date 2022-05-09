using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT1 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 1)");
            Tooltip.SetDefault("Punch enemies at a really fast rate!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
