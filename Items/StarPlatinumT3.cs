using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT3 : StandItemClass
	{
		public override int standSpeed => 7;
		public override int standType => 1;
		public override string standProjectileName => "StarPlatinum";
		public override int standTier => 3;

		public override string Texture
		{
			get { return mod.Name + "/Items/StarPlatinumT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 3)");
			Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 83;
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
			recipe.AddIngredient(mod.ItemType("StarPlatinumT2"));
			recipe.AddIngredient(ItemID.HallowedBar, 14);
			recipe.AddIngredient(ItemID.Amethyst, 4);
			recipe.AddIngredient(ItemID.FallenStar, 6);
			recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
			recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
