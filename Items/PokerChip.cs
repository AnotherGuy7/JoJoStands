using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace JoJoStands.Items
{
	public class PokerChip : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("A poker chip said to have a soul inside...");
		}

		public override void SetDefaults()
        {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 35, 72, 0);
			Item.rare = 8;
            Item.maxStack = 1;
		}

        public override void OnConsumeItem(Player player)
        {
            player.statLife += (player.statLifeMax/2);
        }
    }
}