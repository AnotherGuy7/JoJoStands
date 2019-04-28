using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenT2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (1st Bomb Tier 2)");
			Tooltip.SetDefault("Shoot items that explode and right-click to trigger any block! \nNext Tier: 6 Hallowed Bars, 8 Souls of Might, a Hand");
		}
		public override void SetDefaults()
		{
            item.damage = 47;//Around WoF
            item.melee = true;      //turns out this is used for types, like if you have an armor that does +%5 melee damage, it would affect this, but not an armor with +%5 ranged damage
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 55;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 7, 50, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.maxStack = 1;
            item.crit = 30;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                if (Collision.SolidCollision(Main.MouseWorld, 1, 1))
                {
                    Vector2 velocity = default(Vector2);
                    int projectile = Projectile.NewProjectile(Main.MouseWorld, velocity, ProjectileID.GrenadeIII, 40, 50f, Main.myPlayer);
                    Main.projectile[projectile].friendly = true;
                    Main.projectile[projectile].timeLeft = 2;
                    Main.projectile[projectile].netUpdate = true;

                    return true;
                }
                return false;
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
                item.useStyle = 5;
                item.useTurn = true;
                item.useAnimation = 60;
                item.useTime = 60;
                item.width = 12;
                item.height = 12;
                item.shoot = mod.ProjectileType("KQBomb2Activator");      //because for SOME REASON, you need a projectile
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
            }
            else
            {
                item.damage = 47;
                item.width = 100;
                item.height = 8;
                item.useTime = 13;
                item.useAnimation = 13;
                item.useStyle = 5;
                item.knockBack = 3;
                item.autoReuse = true;
                item.useTurn = true;
                item.shoot = mod.ProjectileType("KQBombFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Dynamite, 12);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}