using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LockT1 : StandItemClass
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
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 1);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}