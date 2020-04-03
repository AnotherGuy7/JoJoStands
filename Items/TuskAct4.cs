using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct4 : ModItem
	{
        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Tusk (ACT 4)");
            Tooltip.SetDefault("Use the infinite energy inside you... \nSpecial: Switch to previous acts!");
		}

		public override void SetDefaults()
		{
			item.damage = 305;
			item.width = 32;
			item.height = 32;
			item.useTime = 15;                      //would need a horse to activate, remember to do this!
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item67;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ControllableNail");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
            item.noUseGraphic = true;
		}

        public bool forceChangedTusk = false;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.TuskActNumber == 4 && player.whoAmI == Main.myPlayer && mPlayer.achievedInfiniteSpin)
            {
                mPlayer.TuskAct4Minion = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct4Minion")] <= 0 && mPlayer.TuskAct4Minion)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct4Minion"), 0, 0f, Main.myPlayer);
                }
            }
            if (mPlayer.goldenSpinCounter > 0)
            {
                if (!UI.GoldenSpinMeter.Visible)
                {
                    UI.GoldenSpinMeter.Visible = true;
                }
                if (mPlayer.achievedInfiniteSpin && !forceChangedTusk)
                {
                    mPlayer.TuskActNumber = 4;
                    forceChangedTusk = true;
                }
                if (mPlayer.goldenSpinCounter <= 1)     //would reset anyway if the player isn't holding Tusk, cause it resets whenever you hold the item again
                {
                    forceChangedTusk = false;
                }
            }
            if (mPlayer.TuskActNumber == 3 && player.whoAmI == Main.myPlayer)
            {
                mPlayer.TuskAct3Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct3Pet")] <= 0 && mPlayer.TuskAct3Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct3Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (mPlayer.TuskActNumber == 2 && player.whoAmI == Main.myPlayer)
            {
                mPlayer.TuskAct2Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct2Pet")] <= 0 && mPlayer.TuskAct2Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct2Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (mPlayer.TuskActNumber == 1 && player.whoAmI == Main.myPlayer)
            {
                mPlayer.TuskAct1Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct1Pet")] <= 0 && mPlayer.TuskAct1Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct1Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (JoJoStands.SpecialHotKey.JustPressed)
            {
                mPlayer.TuskActNumber += 1;
            }
            if (mPlayer.TuskActNumber >= 4 && !mPlayer.achievedInfiniteSpin)
            {
                mPlayer.TuskActNumber = 1;
            }
            if (mPlayer.TuskActNumber >= 5 && mPlayer.achievedInfiniteSpin)
            {
                mPlayer.TuskActNumber = 1;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.TuskActNumber == 4)
            {
                if (player.altFunctionUse == 2)
                {
                    item.useTime = 512;
                    item.useAnimation = 15;
                    item.useStyle = 5;
                    item.knockBack = 4;
                    item.UseSound = SoundID.Item91;
                    item.autoReuse = false;
                    item.shoot = mod.ProjectileType("ReqNail");
                    item.shootSpeed = 30f;
                }
                if (player.altFunctionUse != 2)
                {
                    item.damage = 305;
                    item.useTime = 15;
                    item.useAnimation = 15;
                    item.knockBack = 7;
                    item.autoReuse = false;
                    item.UseSound = SoundID.Item67;
                    item.shoot = mod.ProjectileType("ControllableNail");
                    item.shootSpeed = 60f;
                }
            }
            if (mPlayer.TuskActNumber == 3)
            {
                if (player.altFunctionUse == 2 && player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] == 0)
                {
                    item.useTime = 240;
                    item.useAnimation = 30;
                    item.useStyle = 5;
                    item.autoReuse = false;
                    item.UseSound = SoundID.Item78;
                    item.shoot = mod.ProjectileType("ShadowNail");
                    item.shootSpeed = 60f;
                }
                if (player.altFunctionUse == 2 && player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] != 0)
                {
                    return false;
                }
                if (player.altFunctionUse != 2)
                {
                    item.damage = 122;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.useStyle = 5;
                    item.knockBack = 2f;
                    item.autoReuse = false;
                    item.shoot = mod.ProjectileType("ControllableNail");
                    item.shootSpeed = 60f;
                }
            }
            if (mPlayer.TuskActNumber == 2)
            {
                if (player.altFunctionUse != 2)
                {
                    item.damage = 49;
                    item.width = 100;
                    item.height = 8;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.useStyle = 5;
                    item.knockBack = 4;
                    item.value = 10000;
                    item.rare = 6;
                    item.UseSound = SoundID.Item67;
                    item.autoReuse = false;
                    item.shoot = mod.ProjectileType("ControllableNail");
                    item.maxStack = 1;
                    item.shootSpeed = 40f;
                    item.channel = true;
                }
            }
            if (mPlayer.TuskActNumber == 1)
            {
                if (player.altFunctionUse != 2)
                {
                    item.damage = 21;
                    item.width = 32;
                    item.height = 32;
                    item.useTime = 35;
                    item.useAnimation = 35;
                    item.useStyle = 5;
                    item.knockBack = 4;
                    item.value = 10000;
                    item.rare = 6;
                    item.UseSound = SoundID.Item67;
                    item.autoReuse = true;
                    item.shoot = mod.ProjectileType("Nail");
                    item.maxStack = 1;
                    item.shootSpeed = 30f;
                    item.channel = true;
                }
            }
            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TuskAct3"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
            recipe.AddIngredient(mod.ItemType("DeterminedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}