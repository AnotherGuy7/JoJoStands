using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items
{
	public class PokerChip : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("A poker chip said to have a soul inside...");
		}

		public override void SetDefaults()
        {
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 35, 72, 0);
			item.rare = 8;
            item.maxStack = 1;
		}

        public override void OnConsumeItem(Player player)
        {
            player.statLife += (player.statLifeMax/2);
        }
    }
}