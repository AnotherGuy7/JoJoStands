using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SteelBalls : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steel Balls");
			Tooltip.SetDefault("These steel balls have been passed down from generation to generation... ");
		}
		public override void SetDefaults()
		{
			item.damage = 107;
			item.width = 100;
			item.height = 8;
            item.ranged = true;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 3;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 2;
			item.value = 7;
			item.rare = 6;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("SteelBallP");
			item.shootSpeed = 32f;
            item.useTurn = true;
		}

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SteelBallP")] >= 2)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 2);
            recipe.AddIngredient(ItemID.HallowedBar, 7);
            recipe.AddIngredient(mod.ItemType("Hamon"), 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
