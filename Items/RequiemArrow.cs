using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class RequiemArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Arrow");
            Tooltip.SetDefault("This mysterious arrow is used to maximize some stands' potential.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 11;
            Item.rare = 8;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<ViralPearl>())
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}