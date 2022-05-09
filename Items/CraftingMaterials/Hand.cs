using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.CraftingMaterials
{
	public class Hand : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hand");
			Tooltip.SetDefault("A hand that a certain individual would kill for.");
		}

		public override void SetDefaults()
        {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
            Item.value = Item.buyPrice(0, 0, 24, 0);
            Item.rare = 8;
		}
    }
}