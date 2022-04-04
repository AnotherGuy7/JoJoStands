using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ViralMeteoriteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Meteorite Bar");
            Tooltip.SetDefault("You feel your soul interacting with the metal.");

        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(0, 0, 50, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteorite"), 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}