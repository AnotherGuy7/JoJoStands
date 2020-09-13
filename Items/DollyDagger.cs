using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class DollyDagger : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dolly Dagger");
			Tooltip.SetDefault("Stab yourself to shoot out a damage reflecting beam!");
		}

		public override void SetDefaults()
        {
			item.width = 16;
			item.height = 16;
            item.useTime = 15;
            item.useAnimation = 15;
			item.maxStack = 1;
            item.useStyle = 3;
            item.noUseGraphic = true;
			item.rare = 8;
			item.value = Item.buyPrice(0, 2, 0, 0);
		}

        private int stabDamage = 0;

        public override bool UseItem(Player player)     //non-stab reflection code is in GlobalNPC and in GlobalProjectile
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 targetPos = Main.MouseWorld;
                Vector2 shootVel = targetPos - player.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= 16f;

                stabDamage = Main.rand.Next(50, 81);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't reflect enough damage back."), stabDamage, player.direction);
                Projectile.NewProjectile(player.Center, shootVel, mod.ProjectileType("DollyDaggerBeam"), (int)(stabDamage * 0.7), 2f, Main.myPlayer);
                return true;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
            recipe.AddIngredient(ItemID.Wood, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}