using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
	public class ViralPowder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Viral Meteorite turned into dust. Tastes pretty good!");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 0, 25);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteorite"));
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}