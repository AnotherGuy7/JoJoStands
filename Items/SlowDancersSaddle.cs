
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SlowDancersSaddle : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slow Dancer's Saddle");
			Tooltip.SetDefault("A blue saddle that belongs to a fast race horse...");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.rare = ItemRarityID.Pink;
			item.noMelee = true;
			item.mountType = mod.MountType("SlowDancerMount");
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Leather, 3);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}