using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonT1 : StandItemClass
	{
		public override int standSpeed => 26;
		public override int standType => 1;
		public override string standProjectileName => "KingCrimson";
		public override int standTier => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Tier 1)");
			Tooltip.SetDefault("Donut enemies with a powerful punch!\nConsecutive Donuts deal greater damage.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 42;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToControl"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
