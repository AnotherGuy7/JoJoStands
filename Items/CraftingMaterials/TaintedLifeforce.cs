using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class TaintedLifeforce : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            // DisplayName.SetDefault("Tainted Lifeforce");
            // Tooltip.SetDefault("The aura of someone remorselessly wicked");
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CaringLifeforce>();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 44;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 2);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Items/CraftingMaterials/" + Name);
            DrawAnimation drawAnim = Main.itemAnimations[Item.type];
            Rectangle sourceRect = drawAnim.GetFrame(texture);
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(0f, 1f), sourceRect, Color.White, 0f, new Vector2(texture.Width / 2f, Item.height / 2f), 1f, SpriteEffects.None, 0);     //animates faster than the normal animation???
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.Ectoplasm, 4)
                .Register();
        }
    }
}