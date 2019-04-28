using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class HierophantGreenT2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hierophant Green (Tier 2)");
			Tooltip.SetDefault("Shoot emeralds at the enemies! \nNext Tier: Large Emerald, 12 Chlorophyte Ore");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 11;
			item.useAnimation = 11;
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

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HierophantGreenT1"));
            recipe.AddIngredient(ItemID.Emerald, 2);
            recipe.AddIngredient(ItemID.Hellstone, 10);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
