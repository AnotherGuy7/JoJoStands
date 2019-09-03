using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class GoldExperienceRequiem : ModItem
	{
        public bool saidAbility = false;
        public int regencounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Experience (Requiem)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities! \nSpecial: Switches the abilities used for right-click!");
        }

		public override void SetDefaults()
		{
            item.damage = 138;
            item.width = 32;
            item.height = 32;
            item.useTime = 13;
            item.useAnimation = 13;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 1f;
            item.value = Item.buyPrice(0,58, 26, 82);
            item.rare = 8;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GoldExperienceRequiemFist");
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

        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
                if (JoJoStands.ItemHotKey.JustPressed)
                {
                    Mplayer.GEAbilityNumber += 1;
                    saidAbility = false;
                }
                if (Mplayer.GEAbilityNumber >= 6)
                {
                    Mplayer.GEAbilityNumber = 0;
                }
                if (Mplayer.GEAbilityNumber == 0)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Scorpion Rock");
                        saidAbility = true;
                    }
                }
                if (Mplayer.GEAbilityNumber == 1)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Tree");
                        saidAbility = true;
                    }
                }
                if (Mplayer.GEAbilityNumber == 2)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Death Loop");
                        saidAbility = true;
                    }
                }
                if (Mplayer.GEAbilityNumber == 3)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Limb Recreation");
                        saidAbility = true;
                    }
                    if (Main.mouseRight && player.velocity.X == 0 && player.velocity.Y == 0)
                    {
                        regencounter++;
                    }
                    if (regencounter == 80)
                    {
                        int healamount = 0;
                        healamount = Main.rand.Next(50, 75);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                    }
                }
                if (Mplayer.GEAbilityNumber == 4)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Back to Zero");
                        saidAbility = true;
                    }
                }
                if (regencounter >= 81)
                {
                    regencounter = 0;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 0)
            {
                item.damage = 158;
                item.useTime = 120;
                item.useAnimation = 120;
                item.useStyle = 5;
                item.knockBack = 3f;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("GoldExperienceRock");
                item.shootSpeed = 25f;
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 1 && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !player.HasBuff(mod.BuffType("GEAbilityCooldown")))
            {
                Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, Main.myPlayer, 3f, 0f);
                player.AddBuff(mod.BuffType("GEAbilityCooldown"), 600);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 2 && !player.HasBuff(mod.BuffType("GERAbilityCooldown")) && !player.HasBuff(mod.BuffType("DeathLoop")))
            {
                player.AddBuff(mod.BuffType("DeathLoop"), 1500);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 4 && !player.HasBuff(mod.BuffType("GERAbilityCooldown")) && !player.HasBuff(mod.BuffType("BacktoZero")))
            {
                player.AddBuff(mod.BuffType("BacktoZero"), 1200);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 3)
            {
                return false;
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber != 0)
            {
                item.shoot = 0;
            }
            if (player.altFunctionUse != 2)
            {
                item.damage = 138;
                item.useTime = 10;
                item.useAnimation = 10;
                item.useStyle = 5;
                item.knockBack = 2f;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("GoldExperienceRequiemFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
