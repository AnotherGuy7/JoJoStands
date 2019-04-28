using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class HierophantGreenFinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hierophant Green (Final)");
			Tooltip.SetDefault("Shoot emeralds at the enemies and right click to shoot more accurate emralds!");
		}

		public override void SetDefaults()
		{
			item.damage = 57;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item21;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Emrald");
			item.maxStack = 1;
            item.shootSpeed = 24f;
		}

    	public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (player.altFunctionUse == 2)
			{
				return true;
			}

			float numberProjectiles = 3 + Main.rand.Next(3);
			float rotation = MathHelper.ToRadians(45);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }

		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				item.damage = 57;
				item.ranged = true;
				item.width = 100;
				item.height = 8;
				item.useTime = 10;
				item.useAnimation = 10;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = true;
                item.UseSound = SoundID.Item20;
                item.shoot = mod.ProjectileType("Emrald");
	            item.shootSpeed = 32f;
			}
			else
			{
				item.damage = 57;
				item.ranged = true;
				item.width = 100;
				item.height = 8;
				item.useTime = 10;
				item.useAnimation = 10;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = true;
	        	item.shoot = mod.ProjectileType("Emrald");
	            item.shootSpeed = 24f;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HierophantGreenT2"));
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddIngredient(ItemID.ChlorophyteOre, 12);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
