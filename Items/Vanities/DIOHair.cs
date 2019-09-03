using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Head)]
	public class DIOHair : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DIO's Hair");
            Tooltip.SetDefault("Hair fit for the 'Ruler of Humanity', with a headband.");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.rare = 6;
			item.vanity = true;
		}

		public override bool DrawHead() {
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}