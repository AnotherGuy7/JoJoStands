using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class BadCompanyT3 : StandItemClass
	{
		public override int standSpeed => 70;
		public override int standType => 2;

		public override string Texture
		{
			get { return mod.Name + "/Items/BadCompanyT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bad Company (Tier 3)");
			Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nSpecial: Double the amount of troops around you!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 31;
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
			recipe.AddIngredient(mod.ItemType("BadCompanyT2"));
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.CursedBullet, 150);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 3);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("BadCompanyT2"));
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.IchorBullet, 150);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 3);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
