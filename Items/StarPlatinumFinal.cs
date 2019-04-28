using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace JoJoStands.Items
{
	public class StarPlatinumFinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Final)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and use Star Finger to kill enemies from a distance. Special to stop time for 4 seconds!");
		}
		public override void SetDefaults()
		{
			item.damage = 153;	//thanks Joser for the idea of making this a gun...
			item.width = 100;
			item.height = 8;
			item.useTime = 8;
			item.useAnimation = 4;
			item.useStyle = 5;
			item.maxStack = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 6;
			item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
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

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TheWorldCoolDown")))
            {
                player.AddBuff(mod.BuffType("TheWorldBuff"), 240, true);
            }
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
				item.ranged = true;
				item.width = 100;
				item.height = 8;
				item.useTime = 100;
				item.useAnimation = 10;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("StarFinger");
	            item.shootSpeed = 16f;
			}
			else
			{
				item.damage = 153;
				item.ranged = true;
				item.width = 10;
				item.height = 8;
				item.useTime = 8;
				item.useAnimation = 4;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = true;
	        	item.shoot = mod.ProjectileType("StarPlatinumFist");
	            item.shootSpeed = 50f;
			}
			return true;
		}

        public override bool UseItem(Player player)
        {
            //timer for half a second?
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarPlatinumT3"));
            recipe.AddIngredient(ItemID.ChlorophyteOre, 15);
            recipe.AddIngredient(ItemID.LargeTopaz, 1);
            recipe.AddIngredient(ItemID.FallenStar, 7);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
