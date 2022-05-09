using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Items.CraftingMaterials
{
	public class WillToControl : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will to Control");
            Tooltip.SetDefault("A physical outlook upon the world, heavy and overbearing.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 14));
        }

		public override void SetDefaults()
        {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 0, 7, 0);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Mod.GetTexture("Items/CraftingMaterials/" + Name);
            DrawAnimation drawAnim = Main.itemAnimations[Item.type];
            Rectangle sourceRect = drawAnim.GetFrame(texture);
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(0f, 1f), sourceRect, Color.White, 0f, new Vector2(texture.Width / 2f, Item.height / 2f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }
    }
}