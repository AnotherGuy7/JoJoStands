using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Body)]
	public class BucciaratiShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zipper Patterned Suit");
			Tooltip.SetDefault("A white suit adorned  with several golden zippers, and a large chest window.");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = 6;
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			    .AddIngredient(ItemID.Silk, 10)
			    .AddTile(TileID.Loom)
			    .Register();
		}
	}
}