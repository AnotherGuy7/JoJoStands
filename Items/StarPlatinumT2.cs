using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 2)");
			Tooltip.SetDefault("Punch enemies at a really fast rate. \nNext Tier: 20 Hellstone, 2 Topaz, 5 Fallen Stars");
		}
		public override void SetDefaults()
		{
			item.damage = 82;	//thanks Joser for the idea of making this a gun...
			item.width = 100;
			item.height = 8;
			item.useTime = 11;
			item.useAnimation = 11;
			item.useStyle = 5;
			item.maxStack = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 6;
			item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("StarPlatinumFist");
			item.shootSpeed = 50f;
		}

    	public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float numberProjectiles = 3 + Main.rand.Next(5);
			float rotation = MathHelper.ToRadians(45);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarPlatinumT1"));
            recipe.AddIngredient(ItemID.PlatinumBar, 12);
            recipe.AddIngredient(ItemID.FallenStar, 4);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
