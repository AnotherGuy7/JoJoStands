using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
	public class WillToChange : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will to Change");
            Tooltip.SetDefault("A physical outlook upon the world, morphing and changing.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 15));
        }

		public override void SetDefaults()
        {
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 7, 0);
            item.alpha = 62;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }
    }
}