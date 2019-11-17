using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
	public class WillToControl : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will to Control");
            Tooltip.SetDefault("A physical outlook upon the world, heavy and overbearing.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 14));
        }

		public override void SetDefaults()
        {
			item.width = 24;
			item.height = 24;
			item.maxStack = 99;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 7, 0);
            item.alpha = 125;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }
    }
}