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
			DisplayName.SetDefault("Tusk (Act 2)");
			Tooltip.SetDefault("Shoot controllable spins at enemies! \nNext Tier: 4 Spectre Bars");
		}
		public override void SetDefaults()
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
            item.shoot = mod.ProjectileType("Nail");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TuskAct1"));
            recipe.AddIngredient(ItemID.Hellstone, 12);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}