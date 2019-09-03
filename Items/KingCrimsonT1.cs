using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonT1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Tier 1)");
			Tooltip.SetDefault("Donut enemies with a light-dash donut!");
		}

		public override void SetDefaults()
		{
			item.damage = 31;       //beginning
			item.width = 32;
			item.height = 32;
			item.useTime = 50;
			item.useAnimation = 50;
            item.reuseDelay = 50;
			item.useStyle = 5;
			item.maxStack = 1;
			item.knockBack = 3f;
			item.rare = 6;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("KingCrimsonDonut");
            item.useTurn = true;
            item.shootSpeed = 50f;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)      //UseItem wasn't doing the dashes?
        {
            player.velocity.X = 3f * player.direction;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
