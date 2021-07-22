using Terraria;
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
		public override string standProjectileName => "Cream";
		public override int standTier => 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Final Tier)");
			Tooltip.SetDefault("Chop an enemy with a powerful chop and right-click to envelop yourself in Void!\nSpecial: Completely become a ball of Void and consume everything in your way!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 172;
			item.width = 58;
			item.height = 50;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}

		public override bool ManualStandSpawning(Player player)
		{
			player.GetModPlayer<MyPlayer>().creamTier = standTier;
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CreamT3"));
			recipe.AddIngredient(ItemID.SpectreBar, 21);
			recipe.AddIngredient(ItemID.IceBlock, 25);
			recipe.AddIngredient(mod.ItemType("Tainted Lifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
