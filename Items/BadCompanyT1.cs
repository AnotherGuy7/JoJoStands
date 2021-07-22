using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class BadCompanyT1 : StandItemClass
	{
		public override int standSpeed => 90;
		public override int standType => 2;
		public override string standProjectileName => "BadCompany";
		public override int standTier => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bad Company (Tier 1)");
			Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 9;
			item.width = 46;
			item.height = 50;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

        public override bool ManualStandSpawning(Player player)
        {
			MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

			mPlayer.badCompanyTier = standTier;
			mPlayer.maxBadCompanyUnits = 6 * standTier;
			return false;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToDestroy"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
