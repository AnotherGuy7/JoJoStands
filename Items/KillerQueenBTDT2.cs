using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenBTDT2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (Stray Cat Tier 2)");
			Tooltip.SetDefault("Shoot bubbles that explode and right-click to bite the dust! \nNext Tier: ");
		}
		public override void SetDefaults()
		{
			item.damage = 42;     //Around mid-prehardmode
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 80;
			item.useAnimation = 80;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item85;
			item.autoReuse = false;
            item.shoot = mod.ProjectileType("Bubble");
			item.maxStack = 1;
            item.shootSpeed = 0.7f;
			item.channel = true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Dynamite, 12);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}