
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.mountType = Mod.MountType("RoadRollerMount");
        }

        public override void AddRecipes()
		{
			CreateRecipe()
			    .AddIngredient(ItemID.IronBar, 20);
			    .AddIngredient(ItemID.PalladiumBar, 13);
			    .AddIngredient(ItemID.SoulofNight);

			    .Register();
			CreateRecipe()
			    .AddIngredient(ItemID.LeadBar, 20);
			    .AddIngredient(ItemID.PalladiumBar, 13);
			    .AddIngredient(ItemID.SoulofNight);

			    .Register();
			CreateRecipe()
			    .AddIngredient(ItemID.IronBar, 20);
			    .AddIngredient(ItemID.CobaltBar, 13);
			    .AddIngredient(ItemID.SoulofNight);

			    .Register();
			CreateRecipe()
			    .AddIngredient(ItemID.LeadBar, 20);
			    .AddIngredient(ItemID.CobaltBar, 13);
			    .AddIngredient(ItemID.SoulofNight);

			    .Register();
		}
	}
}