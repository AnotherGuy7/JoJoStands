using Terraria;
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
		public override int standSpeed => 26;
		public override int standType => 1;
		public override string standProjectileName => "Cream";
		public override int standTier => 2;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Tier 2)");
			Tooltip.SetDefault("Chop an enemy with a powerful chop!\nSpecial: Completely become a ball of Void and consume everything in your way!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 68;
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
            recipe.AddIngredient(mod.ItemType("CreamT1"));
			recipe.AddIngredient(ItemID.HellstoneBar, 6);
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
