using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using System;

namespace JoJoStands.UI
{
    internal class AerosmithRadar : UIState
    {
        public DragableUIPanel aerosmithRadarUI;
        public UIImage centerDot;
        public static bool Visible;
        public static Texture2D aerosmithRadarTexture;

        public override void Update(GameTime gameTime)
        {
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

            centerDot = new UIImage(ModContent.GetTexture("JoJoStands/UI/GreenDot"));
            centerDot.Left.Set(58f, 0f);
            centerDot.Top.Set(62f, 0f);
            centerDot.Width.Set(10f, 0f);
            centerDot.Height.Set(10f, 0f);
            aerosmithRadarUI.Append(centerDot);

            Append(aerosmithRadarUI);
            base.OnInitialize();
        }

        private Texture2D redDotTexture;
        private Texture2D orangeDotTexture;
        private int[] dotTimers = new int[Main.maxNPCs];
        private float[] dotAlphas = new float[Main.maxNPCs];

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (redDotTexture == null)
                redDotTexture = ModContent.GetTexture("JoJoStands/UI/RedDot");
            if (orangeDotTexture == null)
                orangeDotTexture = ModContent.GetTexture("JoJoStands/UI/OrangeDot");

            spriteBatch.Draw(aerosmithRadarTexture, aerosmithRadarUI.GetClippingRectangle(spriteBatch), new Rectangle(0, 1, aerosmithRadarTexture.Width, aerosmithRadarTexture.Height), new Color(255f, 255f, 255f, 255f));

            Vector2 aerosmithCenter = Main.projectile[mPlayer.aerosmithWhoAmI].Center;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC target = Main.npc[n];
                if (target.active && target.lifeMax > 5 && !target.townNPC && !target.immortal)
                {
                    float xDistance = aerosmithCenter.X - target.Center.X;
                    float yDistance = aerosmithCenter.Y - target.Center.Y;
                    float xMaxDetectionDistance = Main.screenWidth + 120f;
                    float yMaxDetectionDistance = Main.screenHeight + 120f;

                    if (xDistance < xMaxDetectionDistance && yDistance < yMaxDetectionDistance && xDistance > -xMaxDetectionDistance && yDistance > -yMaxDetectionDistance)
                    {
                        dotTimers[n]++;
                        if (dotTimers[n] >= 360)
                            dotTimers[n] = 0;

                        dotAlphas[n] = 0.5f + (Math.Abs((float)Math.Sin(dotTimers[n] / 6f)) * 0.5f);

                        if (xDistance < 675f && xDistance > -675f && yDistance < 675f && yDistance > -675f)
                        {
                            Rectangle destinationRect = new Rectangle((int)(centerDot.Left.Pixels + 13f + (-xDistance / 18f)) + (int)aerosmithRadarUI.Left.Pixels, (int)(centerDot.Top.Pixels + 13f + (-yDistance / 18f)) + (int)aerosmithRadarUI.Top.Pixels, 10, 10);
                            spriteBatch.Draw(redDotTexture, destinationRect, null, Color.White * dotAlphas[n]);
                        }
                    }
                }
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player detectedPlayer = Main.player[p];
                    if (detectedPlayer.active && detectedPlayer.team != player.team)
                    {
                        float xDistance = aerosmithCenter.X - detectedPlayer.Center.X;
                        float yDistance = aerosmithCenter.Y - detectedPlayer.Center.Y;
                        float xMaxDetectionDistance = Main.screenWidth + 120f;
                        float yMaxDetectionDistance = Main.screenHeight + 120f;

                        if (detectedPlayer.active && xDistance < xMaxDetectionDistance && yDistance < yMaxDetectionDistance && xDistance > -xMaxDetectionDistance && yDistance > -yMaxDetectionDistance)
                        {
                            if (p < Main.maxNPCs)
                            {
                                dotTimers[p]++;     //They use the same ones as NPCs. This'll lead to faster behaviour but just think of it as "increased variation"
                                if (dotTimers[p] >= 360)
                                    dotTimers[p] = 0;

                                dotAlphas[p] = 0.5f + (Math.Abs((float)Math.Sin(dotTimers[p] / 6f)) * 0.5f);
                            }
                            else
                            {
                                p = 0;      //Heh
                            }

                            if (xDistance < 675f && xDistance > -675f && yDistance < 675f && yDistance > -675f)
                            {
                                Rectangle destinationRect = new Rectangle((int)(centerDot.Left.Pixels + 13f + (-xDistance / 18f)) + (int)aerosmithRadarUI.Left.Pixels, (int)(centerDot.Top.Pixels + 13f + (-yDistance / 18f)) + (int)aerosmithRadarUI.Top.Pixels, 10, 10);
                                spriteBatch.Draw(orangeDotTexture, destinationRect, null, Color.White * dotAlphas[p]);
                            }
                        }
                    }
                }
            }
        }
    }
}