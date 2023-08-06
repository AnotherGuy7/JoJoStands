using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class WillToDestroy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Will to Destroy");
            // Tooltip.SetDefault("A physical outlook upon the world, destructive and erratic.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 14));
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WillToChange>();
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 38;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 7, 0);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Items/CraftingMaterials/" + Name).Value;
            DrawAnimation drawAnim = Main.itemAnimations[Item.type];
            Rectangle sourceRect = drawAnim.GetFrame(texture);
            Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition + new Vector2(0f, 1f), sourceRect, Color.White, 0f, new Vector2(texture.Width / 2f, Item.height / 2f), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }
    }
}