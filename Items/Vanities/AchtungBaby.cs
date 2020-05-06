using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Head)]
	public class AchtungBaby : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Makes you invisible!");
		}

		public override void SetDefaults()
        {
			item.width = 18;
			item.height = 18;
			item.rare = 8;
            item.vanity = true;
		}

        public override bool DrawBody()
        {
            return false;
        }

        public override bool DrawHead()
        {
            return false;
        }

        public override bool DrawLegs()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
			player.invis = true;
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