using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TheWorldFinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The World (Final Tier)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to throw knives! \nSpecial: Stop time for 9 seconds!");
		}
		public override void SetDefaults()
		{
            item.damage = 138;
            item.width = 100;
            item.height = 8;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 6;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TheWorldFist");
            item.shootSpeed = 50f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 3 + Main.rand.Next(5);
            float rotation = MathHelper.ToRadians(45);

            if (player.altFunctionUse == 2)
            {
                float rotationk = MathHelper.ToRadians(15);
                float numberKnives = 3;
                position += Vector2.Normalize(new Vector2(speedX, speedY));
                for (int i = 0; i < numberKnives; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
                return true;
            }

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

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TheWorldCoolDown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
                player.AddBuff(mod.BuffType("TheWorldBuff"), 540, true);
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)     // eventually, change this to if (player.altFunctionUse == 2 && MyPlayer.TheWorldEffect == false) and add a if (player.altFunctionUse == 2 && MyPlayer.TheWorldEffect == true) for a Roada Roller Da
            {
                if (player.HasItem(mod.ItemType("Knife")))
                {
                    item.damage = 96;
                    item.ranged = true;
                    item.width = 100;
                    item.height = 8;
                    item.useTime = 13;
                    item.useAnimation = 13;
                    item.useStyle = 5;
                    item.knockBack = 2;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("Knife");
                    player.ConsumeItem(mod.ItemType("Knife"));
                }
                if (!player.HasItem(mod.ItemType("Knife")))
                {
                    return false;
                }
            }
            else
            {
                item.damage = 138;
                item.ranged = true;
                item.width = 10;
                item.height = 8;
                item.useTime = 10;
                item.useAnimation = 10;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("TheWorldFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 26);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 7);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
