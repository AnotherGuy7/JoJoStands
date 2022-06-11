using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class AerosmithRadar : UIState
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

            centerDot = new UIImage(ModContent.Request<Texture2D>("JoJoStands/UI/GreenDot", AssetRequestMode.ImmediateLoad));
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
            if (redDotTexture == null)
                redDotTexture = ModContent.Request<Texture2D>("JoJoStands/UI/RedDot").Value;
            if (orangeDotTexture == null)
                orangeDotTexture = ModContent.Request<Texture2D>("JoJoStands/UI/OrangeDot").Value;
            if (dataPoints == null)
                dataPoints = new AerosmithRadarPoint[Main.maxNPCs];

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
                        AerosmithRadarPoint aerosmithRadarPoint = dataPoints[n];
                        dataPoints[n].timer++;
                        if (dataPoints[n].timer >= 360)
                            dataPoints[n].timer = 0;

                        dataPoints[n].alpha = 0.5f + (Math.Abs((float)Math.Sin(dataPoints[n].timer / 6f)) * 0.5f);
                        dataPoints[n] = aerosmithRadarPoint;

                        if (xDistance < 675f && xDistance > -675f && yDistance < 675f && yDistance > -675f)
                        {
                            Rectangle destinationRect = new Rectangle((int)(centerDot.Left.Pixels + 13f + (-xDistance / 18f)) + (int)aerosmithRadarUI.Left.Pixels, (int)(centerDot.Top.Pixels + 13f + (-yDistance / 18f)) + (int)aerosmithRadarUI.Top.Pixels, 10, 10);
                            spriteBatch.Draw(redDotTexture, destinationRect, null, Color.White * dataPoints[n].alpha);
                        }
                    }
                }
            }
            if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    if (p >= Main.maxNPCs)      //The limit is Main.maxNPCs
                        break;

                    Player detectedPlayer = Main.player[p];
                    if (detectedPlayer.active && detectedPlayer.team != player.team)
                    {
                        float xDistance = aerosmithCenter.X - detectedPlayer.Center.X;
                        float yDistance = aerosmithCenter.Y - detectedPlayer.Center.Y;
                        float xMaxDetectionDistance = Main.screenWidth + 120f;
                        float yMaxDetectionDistance = Main.screenHeight + 120f;

                        if (detectedPlayer.active && xDistance < xMaxDetectionDistance && yDistance < yMaxDetectionDistance && xDistance > -xMaxDetectionDistance && yDistance > -yMaxDetectionDistance)
                        {
                            AerosmithRadarPoint aerosmithRadarPoint = dataPoints[p];
                            dataPoints[p].timer++;
                            if (dataPoints[p].timer >= 360)
                                dataPoints[p].timer = 0;

                            dataPoints[p].alpha = 0.5f + (Math.Abs((float)Math.Sin(dataPoints[p].timer / 6f)) * 0.5f);
                            dataPoints[p] = aerosmithRadarPoint;

                            if (xDistance < 675f && xDistance > -675f && yDistance < 675f && yDistance > -675f)
                            {
                                Rectangle destinationRect = new Rectangle((int)(centerDot.Left.Pixels + 13f + (-xDistance / 18f)) + (int)aerosmithRadarUI.Left.Pixels, (int)(centerDot.Top.Pixels + 13f + (-yDistance / 18f)) + (int)aerosmithRadarUI.Top.Pixels, 10, 10);
                                spriteBatch.Draw(orangeDotTexture, destinationRect, null, Color.White * dataPoints[p].alpha);
                            }
                        }
                    }
                }
            }
        }
    }
}