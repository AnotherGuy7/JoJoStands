using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Body)]
    public class MagentMagentCoat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magent Magent's Coat");
            Tooltip.SetDefault("A bizarre magenta suit, resembling a magician's. Has several pockets for tricks and dynamite.");
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