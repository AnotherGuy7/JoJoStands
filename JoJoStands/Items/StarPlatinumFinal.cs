using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumFinal : StandItemClass
	{
		public override int standSpeed => 6;
		public override int standType => 1;
		public override string standProjectileName => "StarPlatinum";
		public override int standTier => 4;

		public override string Texture
		{
			get { return mod.Name + "/Items/StarPlatinumT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Final)");
			Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nSpecial: Stop time for 4 seconds!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 106;
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
			recipe.AddIngredient(mod.ItemType("StarPlatinumT3"));
			recipe.AddIngredient(ItemID.Ectoplasm, 8);
			recipe.AddIngredient(ItemID.LargeAmethyst, 1);
			recipe.AddIngredient(ItemID.FallenStar, 7);
			recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
