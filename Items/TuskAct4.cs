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
			DisplayName.SetDefault("Tusk (Act 4)");
			Tooltip.SetDefault("Use the infinite energy inside you...");
		}
		public override void SetDefaults()
		{
			item.damage = 1000;
			item.magic = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 15;                      //would need a horse to activate, remember to do this!
			item.useAnimation = 15;
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

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 1000;
                item.ranged = true;
                item.width = 100;
                item.height = 8;
                item.useTime = 120;
                item.useAnimation = 15;
                item.useStyle = 5;
                item.knockBack = 4;
                item.rare = 6;
                item.UseSound = SoundID.Item91;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("ReqNail");
                item.shootSpeed = 40f;
            }
            else
            {
                item.damage = 1000;
                item.ranged = true;
                item.width = 100;
                item.height = 8;
                item.useTime = 15;
                item.useAnimation = 15;
                item.useStyle = 5;
                item.knockBack = 7;
                item.autoReuse = false;
                item.UseSound = SoundID.Item67;
                item.shoot = mod.ProjectileType("Nail");
                item.shootSpeed = 60f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TuskAct3"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}