using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SilverChariotT1 : StandItemClass
	{
		public override int standSpeed => 8;
		public override int standType => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Chariot (Tier 1)");
			Tooltip.SetDefault("stab.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.width = 32;
			item.height = 32;
			item.noUseGraphic = true;
			item.maxStack = 1;
			item.value = 0;
			item.rare = ItemRarityID.LightPurple;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToFight"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
