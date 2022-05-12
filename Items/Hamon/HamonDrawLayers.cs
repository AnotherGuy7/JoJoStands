using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonChargesFrontLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");
            HamonPlayer hamonPlayer = drawPlayer.GetModPlayer<HamonPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 3 && drawPlayer.velocity == Vector2.Zero)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Extras/HamonChargeI").Value;
                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);
                if (hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 2)
                {
                    texture = ModContent.Request<Texture2D>("Extras/HamonChargeII").Value;



                }
                if (hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 1.5)
                {
                    texture = ModContent.Request<Texture2D>("Extras/HamonChargeIII").Value;



                }

                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                int frameHeight = texture.Height / 7;

                hamonPlayer.hamonLayerFrameCounter++;
                if (hamonPlayer.hamonLayerFrameCounter >= 6)
                {
                    hamonPlayer.hamonLayerFrame += 1;
                    hamonPlayer.hamonLayerFrameCounter = 0;
                    if (hamonPlayer.hamonLayerFrame >= 7)
                    {
                        hamonPlayer.hamonLayerFrame = 0;
                    }
                }

                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * hamonPlayer.hamonLayerFrame, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }

    public class DefensiveHamonAuraLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.BackAcc);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod Mod = ModLoader.GetMod("JoJoStands");



            HamonPlayer hamonPlayer = drawPlayer.GetModPlayer<HamonPlayer>();
            if (drawPlayer.active && hamonPlayer.defensiveHamonAuraActive)
            {
                Texture2D auraTexture = ModContent.Request<Texture2D>("Extras/DefensiveHamonAura").Value;



                int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.Position.Y + 20f - Main.screenPosition.Y);

                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                }
                int frameHeight = auraTexture.Height / 7;
                hamonPlayer.defensiveHamonLayerFrameCounter++;
                if (hamonPlayer.defensiveHamonLayerFrameCounter >= 8)
                {
                    hamonPlayer.defensiveHamonLayerFrame += 1;
                    hamonPlayer.defensiveHamonLayerFrameCounter = 0;
                    if (hamonPlayer.defensiveHamonLayerFrame >= 7)
                    {
                        hamonPlayer.defensiveHamonLayerFrame = 0;
                    }
                }

                Vector2 Position = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, frameHeight * hamonPlayer.defensiveHamonLayerFrame, auraTexture.Width, frameHeight);
                Color color = Lighting.GetColor((int)((drawInfo.Position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.Position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(auraTexture.Width / 2f, frameHeight / 2f);

                DrawData drawData = new DrawData(auraTexture, Position, animRect, color, drawPlayer.bodyRotation, origin, 1f, SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}
