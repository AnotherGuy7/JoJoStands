using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct2 : ModItem
	{
        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Tusk (ACT 2)");
			Tooltip.SetDefault("Shoot controllable spins at enemies!\nSpecial: Switch to previous acts!");
		}

		public override void SetDefaults()
		{
			item.damage = 49;
			item.width = 100;
			item.height = 8;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 4;
			item.rare = 6;
			item.UseSound = SoundID.Item67;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ControllableNail");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
            item.noUseGraphic = true;
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
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
            if (mPlayer.TuskActNumber >= 3)
            {
                mPlayer.TuskActNumber = 1;
            }
            base.HoldItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
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
            recipe.AddIngredient(mod.ItemType("TuskAct1"));
            recipe.AddIngredient(ItemID.Hellstone, 12);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}