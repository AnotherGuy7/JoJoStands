using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class CreamT2 : StandItemClass
	{
		public override string Texture
		{
			get { return mod.Name + "/Items/CreamT1"; }
		}
		public override int standSpeed => 24;
		public override int standType => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Tier 2)");
			Tooltip.SetDefault("Chop enemy neck with a powerful punch and right-click to become a Spherical Void!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 96;
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
            recipe.AddIngredient(mod.ItemType("CreamT1"));
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
