using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
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
			item.width = 20;
			item.height = 20;
			item.maxStack = 1;
			item.value = 9;
			item.rare = 8;
		}
    }
}