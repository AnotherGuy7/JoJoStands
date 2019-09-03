using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 3)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and use Star Finger to kill enemies from a distance");
		}

		public override void SetDefaults()
		{
			item.damage = 102;	//thanks Joser for the idea of making this a gun...
			item.width = 100;
			item.height = 8;
			item.useTime = 9;
			item.useAnimation = 9;
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

			if (player.altFunctionUse == 2)
			{
				return true;
			}

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

		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
				item.damage = 89;
				item.useTime = 120;
				item.useAnimation = 120;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("StarFinger");
	            item.shootSpeed = 16f;
			}
			else
			{
				item.damage = 102;
				item.useTime = 9;
				item.useAnimation = 9;
				item.useStyle = 5;
				item.autoReuse = true;
	        	item.shoot = mod.ProjectileType("StarPlatinumFist");
	            item.shootSpeed = 50f;
			}
			return true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarPlatinumT2"));
            recipe.AddIngredient(ItemID.Hellstone, 20);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
