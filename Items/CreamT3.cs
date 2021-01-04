using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class CreamT3 : StandItemClass
	{
		public override string Texture
		{
			get { return mod.Name + "/Items/CreamT1"; }
		}

		public override int standSpeed => 24;
		public override int standType => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Tier 3)");
			Tooltip.SetDefault("Chop an enemy with a powerful chop and right-click to envelop yourself in Void!\nSpecial: Completely become a ball of Void and consume everything in your way!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 116;
			item.width = 58;
			item.height = 50;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CreamT2"));
			recipe.AddIngredient(ItemID.HallowedBar, 7);
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddIngredient(mod.ItemType("Hand"), 2);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 4);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
