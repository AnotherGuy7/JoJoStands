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
        public static int bulletCount = 0;
        public int reloadCounter = 0;
        public int reloadStart = 0;
        public static Terraria.Audio.LegacySoundStyle usesound;

        public override string Texture
        {
            get { return mod.Name + "/Items/SexPistolsT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sex Pistols (Final)");
			Tooltip.SetDefault("Shoot homing bullets at your enemie and right click to shoot all 6 bullets!\nSpecial: Quickened Reloading");
		}
		public override void SetDefaults()
		{
			item.damage = 187;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 1;
			item.useAnimation = 1;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = usesound;
            item.autoReuse = false;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = mod.ProjectileType("SPBullet");
            item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 6;
			float rotation = MathHelper.ToRadians(4);
            if (type != mod.ProjectileType("SPBullet"))
            {
                type = mod.ProjectileType("SPBullet");
            }

            if (player.altFunctionUse == 2 && bulletCount == 0)
			{
                bulletCount = 6;
                position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f; // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				}
                return true;
			}
            bulletCount += 1;
            return true;
		}

        public override void HoldItem(Player player)
        {
            reloadCounter--;
            if (bulletCount == 6)       //do you really need this line?
            {
                reloadStart++;
            }
            if (bulletCount != 6)
            {
                reloadStart = 0;
            }
            if (reloadStart == 1)
            {
                if (MyPlayer.Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Reload60"));
                }
                if (player.altFunctionUse == 2)
                {
                    reloadCounter = 180;
                }
                else
                {
                    reloadCounter = 60;
                }
            }
            if (JoJoStands.ItemHotKey.JustPressed && reloadCounter <= 1 && player.whoAmI == Main.myPlayer)
            {
                if (MyPlayer.Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Reload30"));
                }
                reloadCounter = 30;
            }
            if (reloadCounter <= 0)
            {
                reloadCounter = 0;
            }
            if (reloadCounter == 1)
            {
                bulletCount = 0;
            }
            if (!MyPlayer.Sounds)
            {
                usesound = SoundID.Item38;
            }
            UI.BulletCounter.Visible = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2 && bulletCount == 0)
            {
				item.damage = 87;
				item.useTime = 5;
				item.useAnimation = 5;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("SPBullet");
	            item.shootSpeed = 40f;
            }
			if (player.altFunctionUse != 2)
			{
				item.damage = 187;
				item.useTime = 2;
				item.useAnimation = 2;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("SPBullet");
	            item.shootSpeed = 40f;
            }
            if ((reloadCounter >= 1) || (player.altFunctionUse == 2 && bulletCount != 0))
            {
                return false;
            }
            else
            {
                return true;
            }
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