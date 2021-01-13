using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class BadCompanyT2 : StandItemClass
	{
		public override int standSpeed => 80;
		public override int standType => 2;

		public override string Texture
		{
			get { return mod.Name + "/Items/BadCompanyT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bad Company (Tier 2)");
			Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 46;
			item.height = 50;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(ItemID.HellstoneBar, 8);
			recipe.AddIngredient(ItemID.EmptyBullet, 50);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
