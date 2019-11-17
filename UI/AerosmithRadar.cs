using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class AerosmithRadar : UIState
    {
        public DragableUIPanel aerosmithRadarUI;
        public UIImage centerDot;
        public static bool Visible;
        public static Texture2D aerosmithRadarTexture;
        public int radarRefreshTimer = 0;

        public override void Update(GameTime gameTime)
        {
            radarRefreshTimer++;

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

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D redDot = ModContent.GetTexture("JoJoStands/UI/RedDot");
            spriteBatch.Draw(aerosmithRadarTexture, aerosmithRadarUI.GetClippingRectangle(spriteBatch), new Rectangle(0, 1, aerosmithRadarTexture.Width, aerosmithRadarTexture.Height), new Color(255f, 255f, 255f, 255f));
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                if (Main.npc[k].active)
                {
                    float xDistance = Main.projectile[Projectiles.Minions.Aerosmith.aerosmithWhoAmI].position.X - Main.npc[k].position.X;       //gettng 
                    float yDistance = Main.projectile[Projectiles.Minions.Aerosmith.aerosmithWhoAmI].position.Y - Main.npc[k].position.Y;
                    float xMaxDetectionDistance = Main.screenWidth + 120f;
                    float yMaxDetectionDistance = Main.screenHeight + 120f;
                    if (Main.npc[k].active && Main.npc[k].lifeMax > 5 && !Main.npc[k].townNPC && xDistance < xMaxDetectionDistance && yDistance < yMaxDetectionDistance && xDistance > -xMaxDetectionDistance && yDistance > -yMaxDetectionDistance)
                    {
                        spriteBatch.Draw(redDot, new Rectangle((int)(centerDot.Left.Pixels + 13f + (-xDistance / 18f)) + (int)aerosmithRadarUI.Left.Pixels, (int)(centerDot.Top.Pixels + 13f + (-yDistance / 18f)) + (int)aerosmithRadarUI.Top.Pixels, 10, 10), new Rectangle(0, 1, 10, 10), new Color(255f, 255f, 255f, 255f));
                    }
                }
            }
        }
    }
}