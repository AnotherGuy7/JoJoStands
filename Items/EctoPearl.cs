using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using JoJoStands.Buffs.ItemBuff;

namespace JoJoStands.Items
{
	public class EctoPearl : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ecto Pearl");
			Tooltip.SetDefault("A otherworldly pearl that has been inherited by a trapped soul from the dungeon.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.value = Item.buyPrice(0, 0, 0, 60);
			item.rare = ItemRarityID.Orange;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useTurn = true;
		}
		public override bool UseItem(Player player)
		{
			player.GetModPlayer<MyPlayer>().standRangeBoosts += 8f;
			player.GetModPlayer<MyPlayer>().usedEctoPearl = true;
			player.ConsumeItem(mod.ItemType("EctoPearl"));
			return base.UseItem(player);
		}
	}
}