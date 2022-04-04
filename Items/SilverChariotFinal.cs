using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SilverChariotFinal : StandItemClass
	{
		public override int standSpeed => 5;
		public override int standType => 1;
		public override string standProjectileName => "SilverChariot";
		public override int standTier => 4;

		public override string Texture
		{
			get { return mod.Name + "/Items/SilverChariotT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Chariot (Final Tier)");
			Tooltip.SetDefault("Left-click to stab enemies and right-click to parry enemies and projectiles away!\nSpecial: Take SC's armor off! (Reduces player defense by 40%)\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 76;
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
			recipe.AddIngredient(mod.ItemType("SilverChariotT3"));
			recipe.AddIngredient(ItemID.ChlorophyteBar, 14);
			recipe.AddIngredient(ItemID.Ectoplasm, 5);
			recipe.AddIngredient(ItemID.GoldBar, 4);
			recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
			recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
			recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("SilverChariotT3"));
			recipe.AddIngredient(ItemID.ChlorophyteBar, 14);
			recipe.AddIngredient(ItemID.Ectoplasm, 5);
			recipe.AddIngredient(ItemID.PlatinumBar, 4);
			recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
			recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
			recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
