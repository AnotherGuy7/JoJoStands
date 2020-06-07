using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
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
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 11;
			item.rare = 8;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("ViralPearl"));
            recipe.AddTile(TileID.AncientManipulator);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}