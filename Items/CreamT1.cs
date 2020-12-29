using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class CreamT1 : StandItemClass
	{
		public override int standSpeed => 25;
		public override int standType => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Tier 1)");
			Tooltip.SetDefault("Chop enemy neck with a powerful punch!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 64;
			item.width = 86;
			item.height = 74;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToDestroy"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
