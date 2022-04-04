using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT1 : StandItemClass
	{
		public override int standSpeed => 9;
		public override int standType => 1;
		public override string standProjectileName => "StarPlatinum";
		public override int standTier => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 1)");
			Tooltip.SetDefault("Punch enemies at a really fast rate.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 22;
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
