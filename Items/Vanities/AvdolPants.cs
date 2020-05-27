using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Legs)]
	public class AvdolPants : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fortuneteller Pants");
			Tooltip.SetDefault("A pair of pants with a grey waist piece.");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 6;
			item.vanity = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}