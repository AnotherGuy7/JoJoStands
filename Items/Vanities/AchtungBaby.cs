using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


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
			Item.width = 18;
			Item.height = 18;
			Item.rare = 8;
            Item.vanity = true;
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
			CreateRecipe()
			    .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Loom)
			    .Register();
		}
	}
}