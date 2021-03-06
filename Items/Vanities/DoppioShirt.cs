using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Body)]
    public class DoppioShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doppio's Shirt");
            Tooltip.SetDefault("A purple shirt with many holes in it. Looks like it's been ripped from the inside multiple times...");
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
			recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}