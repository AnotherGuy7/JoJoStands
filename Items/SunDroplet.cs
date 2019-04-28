using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class SunDroplet : ModItem
	{
		public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A warm droplet... It seems to react to you");
            DisplayName.SetDefault("Sun Droplet");
		}

		public override void SetDefaults()
        {
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.rare = 2;
			item.value = Item.buyPrice(0, 0, 0, 20);
		}
    }
}