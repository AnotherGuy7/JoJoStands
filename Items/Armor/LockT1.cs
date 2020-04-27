using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class LockT1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lock (Tier 1)");
            Tooltip.SetDefault("Make people that harm you overwhelmed with Guilt!");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 6;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}