using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralYoYo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viral Yoyo");
			Tooltip.SetDefault("A deadly yoyo with small spikes to cut and infect your enemies.\nInflicts Infected upon enemy hit.");
		}

		public override void SetDefaults()
		{
			item.damage = 9;
			item.width = 24;
			item.height = 24;
			item.useAnimation = 25;
			item.useTime = 25;
			item.knockBack = 4f;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.shoot = mod.ProjectileType("ViralYoYoProjectile");
			item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 3);
			recipe.AddIngredient(ItemID.WhiteString);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
