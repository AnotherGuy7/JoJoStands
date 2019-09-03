using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/TuskAct1"; }
        }

        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Tusk (ACT 3)");
			Tooltip.SetDefault("Shoot controllable spins at enemies and right-click to teleport where a shot lands!\nSpecial: Switch to previous acts!");
		}

		public override void SetDefaults()
		{
			item.damage = 184;
			item.magic = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 4f;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item67;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ControllableNail");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
		}

        public override void HoldItem(Player player)
        {
            if (player.GetModPlayer<MyPlayer>().TuskActNumber == 3 && player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<MyPlayer>().TuskAct3Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct3Pet")] <= 0 && player.GetModPlayer<MyPlayer>().TuskAct3Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct3Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (player.GetModPlayer<MyPlayer>().TuskActNumber == 2 && player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<MyPlayer>().TuskAct2Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct2Pet")] <= 0 && player.GetModPlayer<MyPlayer>().TuskAct2Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct2Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (player.GetModPlayer<MyPlayer>().TuskActNumber == 1 && player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<MyPlayer>().TuskAct1Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct1Pet")] <= 0 && player.GetModPlayer<MyPlayer>().TuskAct1Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct1Pet"), 0, 0f, Main.myPlayer);
                }
            }
            if (JoJoStands.ItemHotKey.JustPressed)
            {
                player.GetModPlayer<MyPlayer>().TuskActNumber += 1;
            }
            if (player.GetModPlayer<MyPlayer>().TuskActNumber >= 4)
            {
                player.GetModPlayer<MyPlayer>().TuskActNumber = 1;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] == 0 && player.GetModPlayer<MyPlayer>().TuskActNumber == 3)
            {
                item.useTime = 240;
                item.useAnimation = 30;
                item.useStyle = 5;
                item.autoReuse = false;
                item.UseSound = SoundID.Item78;
                item.shoot = mod.ProjectileType("ShadowNail");
                item.shootSpeed = 60f;
            }
            if (player.altFunctionUse != 2 && player.GetModPlayer<MyPlayer>().TuskActNumber == 3)
            {
                item.damage = 184;
                item.ranged = true;
                item.useTime = 30;
                item.useAnimation = 30;
                item.useStyle = 5;
                item.knockBack = 2f;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("ControllableNail");
                item.shootSpeed = 60f;
            }
            if (player.altFunctionUse != 2 && player.GetModPlayer<MyPlayer>().TuskActNumber == 2)
            {
                item.damage = 93;
                item.magic = true;
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
            if (player.altFunctionUse != 2 && player.GetModPlayer<MyPlayer>().TuskActNumber == 1)
            {
                item.damage = 17;
                item.magic = true;
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
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TuskAct2"));
            recipe.AddIngredient(ItemID.SpectreBar, 4);
            recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}