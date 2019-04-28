using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StandArrow : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Use this to create Stands!");
		}

		public override void SetDefaults()
        {
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 8;
			item.value = Item.buyPrice(0, 86, 54, 25);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}