
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class RoadRollerKeys : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Road Roller Keys");
			Tooltip.SetDefault("Keys for a Road Roller!");
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
			item.noUseGraphic = true;
			item.mountType = mod.MountType("RoadRollerMount");
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 20);
			recipe.AddIngredient(ItemID.PalladiumBar, 13);
			recipe.AddIngredient(ItemID.SoulofNight);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}