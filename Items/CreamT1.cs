using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class CreamT1 : StandItemClass
	{
		public override int standSpeed => 28;
		public override int standType => 1;
		public override string standProjectileName => "Cream";
		public override int standTier => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cream (Tier 1)");
			Tooltip.SetDefault("Chop an enemy with a powerful chop!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 35;
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
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToDestroy"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
