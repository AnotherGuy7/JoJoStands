using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ChlorositeBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Bar");
            Tooltip.SetDefault("A gem of nature corrupted by an otherworldly virus...");
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 1, 90, 0);
            item.rare = 1;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteorite"), 2);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}