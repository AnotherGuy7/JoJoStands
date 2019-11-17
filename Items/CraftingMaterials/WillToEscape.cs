using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
	public class WillToEscape : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will to Escape");
            Tooltip.SetDefault("A physical outlook upon the world, fleeting and vague.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 15));
        }

		public override void SetDefaults()
        {
			item.width = 26;
			item.height = 38;
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