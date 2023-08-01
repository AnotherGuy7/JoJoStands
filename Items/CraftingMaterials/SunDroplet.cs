using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class SunDroplet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A warm droplet... It seems to react to you.\nRight-click while holding more than 20 to gain a Hamon SKill Point.\nUsed for creating Hamon weapons.");
            Item.ResearchUnlockCount = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(copper: 20);
        }

        private int glowmaskTimer = 0;
        private float glowmaskAlpha = 0f;

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0.6f;
            Lighting.AddLight(Item.Center, new Vector3(0.5f, 0.5f, 0.1f) / 2f);

            glowmaskTimer += 2;
            if (glowmaskTimer >= 360)
                glowmaskTimer = 0;

            glowmaskAlpha = (float)Math.Abs(Math.Sin(MathHelper.ToRadians(glowmaskTimer)));
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Items/CraftingMaterials/" + Name).Value;
            Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, null, Color.White * glowmaskAlpha, rotation, new Vector2(Item.width / 2f, Item.height / 2f), scale, SpriteEffects.None, 0);
        }
    }
}