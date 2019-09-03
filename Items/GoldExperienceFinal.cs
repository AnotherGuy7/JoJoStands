using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class GoldExperienceFinal : ModItem
	{
        public bool saidAbility = false;
        public int regencounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Experience (Final Tier)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities! \nSpecial: Switches the abilities used for right-click!");
        }

		public override void SetDefaults()
		{
            item.damage = 98;
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
            item.shoot = mod.ProjectileType("GoldExperienceFist");
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
                if (Mplayer.GEAbilityNumber >= 4)
                {
                    Mplayer.GEAbilityNumber = 0;
                }
                if (Mplayer.GEAbilityNumber == 0)
                {
                    if (!saidAbility)
                    {
                        Main.NewText("Ability: Frog");
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
                        Main.NewText("Ability: Butterflies");
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
                    if (regencounter == 120)
                    {
                        int healamount = 0;
                        healamount = Main.rand.Next(25, 50);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                    }
                }
                if (regencounter >= 121)
                {
                    regencounter = 0;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 0 && !player.HasBuff(mod.BuffType("GEAbilityCooldown")))
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GEFrog"), 1, 0f, Main.myPlayer, 3f, 3f);
                player.AddBuff(mod.BuffType("GEAbilityCooldown"), 240);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 1 && !player.HasBuff(mod.BuffType("GEAbilityCooldown")) && Collision.SolidCollision(Main.MouseWorld, 1, 1))
            {
                Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, Main.myPlayer, 2f, 0f);
                player.AddBuff(mod.BuffType("GEAbilityCooldown"), 660);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 2 && !player.HasBuff(mod.BuffType("GEAbilityCooldown")))
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GEButterfly"), 1, 0f, Main.myPlayer);
                player.AddBuff(mod.BuffType("GEAbilityCooldown"), 720);
            }
            if (player.altFunctionUse == 2 && Mplayer.GEAbilityNumber == 3)
            {
                return false;
            }
            if (player.altFunctionUse == 2)
            {
                item.shoot = 0;
            }
            if (player.altFunctionUse != 2)
            {
                item.damage = 138;
                item.ranged = true;
                item.width = 10;
                item.height = 8;
                item.useTime = 10;
                item.useAnimation = 10;
                item.useStyle = 5;
                item.knockBack = 2f;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("GoldExperienceFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GoldExperienceT3"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddIngredient(ItemID.LifeFruit, 2);
            recipe.AddIngredient(ItemID.RedHusk, 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
