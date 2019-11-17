using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 1)");
			Tooltip.SetDefault("Punch enemies at a really fast rate.");
		}

		public override void SetDefaults()
		{
			item.damage = 22;	//thanks Joser for the idea of making this a gun...
			item.width = 32;
			item.height = 32;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 3f;
			item.value = 0;
			item.rare = 6;
            MyPlayer.standTier1List.Add(mod.ItemType(Name));
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.whoAmI == Main.myPlayer)
            {
                mPlayer.StandOut = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("StarPlatinumStand")] <= 0 && mPlayer.StandOut)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarPlatinumStand"), 0, 0f, Main.myPlayer);
                }
            }
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
