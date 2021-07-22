using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class BadCompanyFinal : StandItemClass
	{
		public override int standSpeed => 60;
		public override int standType => 2;
		public override string standProjectileName => "BadCompany";
		public override int standTier => 4;

		public override string Texture
		{
			get { return mod.Name + "/Items/BadCompanyT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bad Company (Final Tier)");
			Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nSpecial: Order an arial bombing on the land around you and call in soldier reinforcements!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
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
			recipe.AddIngredient(mod.ItemType("BadCompanyT3"));
			recipe.AddIngredient(ItemID.Ectoplasm, 4);
			recipe.AddIngredient(ItemID.ChlorophyteBullet, 200);
			recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
