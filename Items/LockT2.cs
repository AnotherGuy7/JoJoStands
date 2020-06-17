using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LockT2 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/LockT1"; }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lock (Tier 2)");
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