using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tusk (Act 1)");
			Tooltip.SetDefault("Shoot nails at enemies! \nNext Tier: 12 Hellstone");
		}
		public override void SetDefaults()
		{
			item.damage = 17;
			item.magic = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item67;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("Nailv1");
			item.maxStack = 1;
            item.shootSpeed = 40f;
			item.channel = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}