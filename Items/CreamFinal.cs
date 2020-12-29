using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class CreamFinal : StandItemClass
	{
		public override string Texture
		{
			get { return mod.Name + "/Items/CreamT1"; }
		}
		public override int standSpeed => 22;
		public override int standType => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Final Tier)");
			Tooltip.SetDefault("Chop enemy neck with a powerful punch and right-click to become a Spherical Void!\nSpecial: Take a peek!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 216;
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
			recipe.AddIngredient(mod.ItemType("CreamT3"));
			recipe.AddIngredient(ItemID.SpectreBar, 21);
			recipe.AddIngredient(ItemID.IceBlock);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 8);
			recipe.AddIngredient(mod.ItemType("Tainted Lifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
