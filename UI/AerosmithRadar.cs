using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class AerosmithRadar : UIState
    {
        public DragableUIPanel aerosmithRadarUI;
        public UIImage centerDot;
        public static bool Visible;
        public static Texture2D aerosmithRadarBorderTexture;
        public static Texture2D aerosmithRadarCrosshairTexture;
        public static Texture2D aerosmithRadarBackgroundTexture;
        public static Texture2D aerosmithRadarBlipTexture;
        private const int AerosmithBlipTextureSize = 6;

        private readonly Color DarkGreen = new Color(27, 76, 32);

        public override void Update(GameTime gameTime)
        {
            centerDot.HAlign = 0.5f * (1f / Main.UIScale);

            centerDot.VAlign = 0.5f * (1f / Main.UIScale);
            centerDot.VAlign += (1 * 8) / 150f;
            centerDot.ImageScale = 4f * (1f / Main.UIScale);
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            aerosmithRadarUI = new DragableUIPanel();
            aerosmithRadarUI.Left.Set(800f, 0f);
            aerosmithRadarUI.Top.Set(510f, 0f);
            aerosmithRadarUI.Width.Set(150f, 0f);
            aerosmithRadarUI.Height.Set(150f, 0f);
            aerosmithRadarUI.BackgroundColor = new Color(0, 0, 0, 0);
            aerosmithRadarUI.BorderColor = new Color(0, 0, 0, 0);

            centerDot = new UIImage(aerosmithRadarBlipTexture);
            centerDot.HAlign = 0.5f;
            centerDot.VAlign = 0.5f;
            centerDot.Width.Set(6f, 0f);
            centerDot.Height.Set(6f, 0f);
            centerDot.ImageScale = 4f;
            centerDot.Color = Color.LightGreen;
            aerosmithRadarUI.Append(centerDot);

            Append(aerosmithRadarUI);
            base.OnInitialize();
        }

        private AerosmithRadarPoint[] dataPoints;

        private struct AerosmithRadarPoint
        {
            public int timer;
            public float alpha;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (dataPoints == null)
                dataPoints = new AerosmithRadarPoint[Main.maxNPCs];

            float scaleInverse = 1f - (Main.UIScale - 1f);
            Rectangle clippingRect = aerosmithRadarUI.GetClippingRectangle(spriteBatch);
            Point transformedPosition = Vector2.Transform(clippingRect.Location.ToVector2(), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
            Rectangle mainUIdestinationRect = new Rectangle(transformedPosition.X, transformedPosition.Y, (int)(clippingRect.Width * scaleInverse), (int)(clippingRect.Height * scaleInverse));

            //return;
            MiscShaderData staticShader = JoJoStandsShaders.GetShaderInstance(JoJoStandsShaders.MultiColorStaticEffect);
            staticShader.UseOpacity(Main.rand.Next(0, 1000 + 1) / 1000f);
            staticShader.UseColor(DarkGreen);
            staticShader.UseSecondaryColor(Color.Black);

            UITools.DrawUIWithShader(spriteBatch, staticShader, aerosmithRadarBackgroundTexture, mainUIdestinationRect);

            spriteBatch.Draw(aerosmithRadarCrosshairTexture, mainUIdestinationRect, Color.White);

            Vector2 aerosmithCenter = Main.projectile[mPlayer.aerosmithWhoAmI].Center;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC target = Main.npc[n];
                if (target.active && target.lifeMax > 5 && !target.townNPC && !target.immortal)
                {
                    Vector2 enemyDistance = target.Center - aerosmithCenter;
                    Vector2 maxDetectionDistance = new Vector2((Main.screenWidth / Main.GameZoomTarget), (Main.screenHeight / Main.GameZoomTarget)) / (Main.GameZoomTarget);

                    if (Math.Abs(enemyDistance.X) < maxDetectionDistance.X && Math.Abs(enemyDistance.Y) < maxDetectionDistance.Y)
                    {
                        AerosmithRadarPoint aerosmithRadarPoint = dataPoints[n];
                        aerosmithRadarPoint.timer++;
                        if (aerosmithRadarPoint.timer >= 360)
                            aerosmithRadarPoint.timer = 0;

                        aerosmithRadarPoint.alpha = 0.5f + (Math.Abs((float)Math.Sin(aerosmithRadarPoint.timer / 6f)) * 0.5f);
                        dataPoints[n] = aerosmithRadarPoint;

                        int dotPosX = clippingRect.X + (int)centerDot.Left.Pixels + (int)((enemyDistance.X / maxDetectionDistance.X) * ((aerosmithRadarBorderTexture.Width - (4 * 6)) * scaleInverse));
                        int dotPosY = clippingRect.Y + (int)centerDot.Top.Pixels + (int)((enemyDistance.Y / maxDetectionDistance.Y) * ((aerosmithRadarBorderTexture.Height - (3 * 6)) * scaleInverse)) + 2;
                        Point transformedDotPosition = Vector2.Transform(new Vector2(dotPosX, dotPosY), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
                        Rectangle destinationRect = new Rectangle(transformedDotPosition.X + 3, transformedDotPosition.Y + 3, (int)(AerosmithBlipTextureSize * scaleInverse), (int)(AerosmithBlipTextureSize * scaleInverse));      //The +3 is because of the origin of the texture.
                        spriteBatch.Draw(aerosmithRadarBlipTexture, destinationRect, Color.Red * dataPoints[n].alpha);
                    }
                }
            }
            if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    if (p >= Main.maxNPCs)      //The limit is Main.maxNPCs
                        break;

                    Player detectedPlayer = Main.player[p];
                    if (detectedPlayer.active && detectedPlayer.team != player.team)
                    {
                        Vector2 playerDistance = detectedPlayer.Center - aerosmithCenter;
                        Vector2 maxDetectionDistance = new Vector2((Main.screenWidth / Main.GameZoomTarget), (Main.screenHeight / Main.GameZoomTarget)) / (Main.GameZoomTarget);

                        if (Math.Abs(playerDistance.X) < maxDetectionDistance.X && Math.Abs(playerDistance.Y) < maxDetectionDistance.Y)
                        {
                            AerosmithRadarPoint aerosmithRadarPoint = dataPoints[p];
                            aerosmithRadarPoint.timer++;
                            if (aerosmithRadarPoint.timer >= 360)
                                aerosmithRadarPoint.timer = 0;

                            aerosmithRadarPoint.alpha = 0.5f + (Math.Abs((float)Math.Sin(aerosmithRadarPoint.timer / 6f)) * 0.5f);
                            dataPoints[p] = aerosmithRadarPoint;

                            int dotPosX = clippingRect.X + (int)centerDot.Left.Pixels + (int)((playerDistance.X / maxDetectionDistance.X) * ((aerosmithRadarBorderTexture.Width - (4 * 6)) * scaleInverse));
                            int dotPosY = clippingRect.Y + (int)centerDot.Top.Pixels + (int)((playerDistance.Y / maxDetectionDistance.Y) * ((aerosmithRadarBorderTexture.Height - (3 * 6)) * scaleInverse)) + 2;
                            Point transformedDotPosition = Vector2.Transform(new Vector2(dotPosX, dotPosY), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
                            Rectangle destinationRect = new Rectangle(transformedDotPosition.X + 3, transformedDotPosition.Y + 3, (int)(AerosmithBlipTextureSize * scaleInverse), (int)(AerosmithBlipTextureSize * scaleInverse));      //The +3 is because of the origin of the texture.
                            spriteBatch.Draw(aerosmithRadarBlipTexture, destinationRect, Color.Orange * dataPoints[p].alpha);
                        }
                    }
                }
            }

            spriteBatch.Draw(aerosmithRadarBorderTexture, mainUIdestinationRect, Color.White);
        }
    }
}