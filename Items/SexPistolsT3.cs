using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SexPistolsT3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sex Pistols (Tier 3)");
			Tooltip.SetDefault("Shoot homing bullets at your enemies! \nNext Tier: 2 Luminite Bars");
		}
		public override void SetDefaults()
		{
			item.damage = 139;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item38;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("SPBullet");
			item.maxStack = 1;
            item.shootSpeed = 35f;
			item.channel = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("SexPistolsT2"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 3);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}