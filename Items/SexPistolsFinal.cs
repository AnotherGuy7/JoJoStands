using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SexPistolsFinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sex Pistols (Final)");
			Tooltip.SetDefault("Shoot homing bullets at your enemie and right click to shoot all 6 bullets!");
		}
		public override void SetDefaults()
		{
			item.damage = 187;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item38;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("SPBullet");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 6;
			float rotation = MathHelper.ToRadians(4);

			if (player.altFunctionUse == 2)
			{
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
				for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
					}
				return true;
			}
			return true;
		}

		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				item.damage = 87;
				item.ranged = true;
				item.width = 100;
				item.height = 8;
				item.useTime = 450;
				item.useAnimation = 10;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("SPBullet");
	            item.shootSpeed = 40f;
			}
			else
			{
				item.damage = 187;
				item.ranged = true;
				item.width = 10;
				item.height = 8;
				item.useTime = 30;
				item.useAnimation = 4;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("SPBullet");
	            item.shootSpeed = 40f;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("SexPistolsT3"));
            recipe.AddIngredient(ItemID.LunarBar, 2);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}