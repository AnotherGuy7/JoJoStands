using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Items.CraftingMaterials
{
	public class WillToProtect : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will to Protect");
            Tooltip.SetDefault("A physical outlook upon the world, defensive and caring.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 15));
        }

		public override void SetDefaults()
        {
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 7, 0);
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/CraftingMaterials/" + Name);
            DrawAnimation drawAnim = Main.itemAnimations[item.type];
            Rectangle sourceRect = drawAnim.GetFrame(texture);
            spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0f, 1f), sourceRect, Color.White, 0f, new Vector2(texture.Width / 2f, item.height / 2f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }
    }
}