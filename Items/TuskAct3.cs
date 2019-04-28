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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tusk (Act 3)");
			Tooltip.SetDefault("Shoot controllable spins at enemies and right-click to teleport where a shot lands! \nNext Tier: Requiem Arrow");
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
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item67;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Nail");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                return false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 240;
                if (Main.myPlayer == player.whoAmI)
                {
                    player.position = Main.MouseWorld;
                }
            }
            else
            {
                item.damage = 184;
                item.ranged = true;
                item.width = 100;
                item.height = 8;
                item.useTime = 30;
                item.useAnimation = 30;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("Nail");
                item.shootSpeed = 60f;
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