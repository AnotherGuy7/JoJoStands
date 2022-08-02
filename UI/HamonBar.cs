using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class HamonBar : UIState
    {
        public static bool visible;
        public static int sizeMode;

        public DragableUIPanel hamonBar;
        public UIText hamonDisplay;
        public static Texture2D hamonBarTexture;
        public static bool changedInConfig = false;

        public static void ShowHamonBar()
        {
            visible = true;
        }

        public static void HideHamonBar()
        {
            visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (!visible)
            {
                base.Update(gameTime);
                return;
            }

            if (sizeMode >= 4)
                sizeMode = 3;
            if (sizeMode <= 0)
            {
                sizeMode = 0;
                visible = false;
            }

            hamonBar.Width.Set(70f + (70 * sizeMode), 0f);
            hamonBar.Height.Set(48f + (48 * sizeMode), 0f);
            hamonDisplay.Left.Set(5f + (30 * sizeMode), 0f);
            hamonDisplay.Top.Set(30f + (40 * sizeMode), 0f);

            hamonDisplay.SetText(hamonPlayer.amountOfHamon + "/" + hamonPlayer.maxHamon);
            if (changedInConfig)
            {
                hamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
                hamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
                changedInConfig = false;
            }


            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            hamonBar = new DragableUIPanel();
            hamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
            hamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
            hamonBar.Width.Set(140f, 0f);
            hamonBar.Height.Set(96f, 0f);
            hamonBar.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            hamonBar.BorderColor = new Color(0, 0, 0, 0);

            hamonDisplay = new UIText(0 + "/" + 0);
            hamonDisplay.Left.Set(70f, 0f);
            hamonDisplay.Top.Set(150f, 0f);
            hamonDisplay.Width.Set(22f, 0f);
            hamonDisplay.Height.Set(22f, 0f);
            hamonBar.Append(hamonDisplay);

            Append(hamonBar);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            int frame = (int)MathHelper.Clamp(hamonPlayer.amountOfHamon / 12, 1, 23) - 1;
            int frameHeight = hamonBarTexture.Height / 24;      //24 frames in that sheet
            /*if (hamonPlayer.amountOfHamon >= 3 && hamonPlayer.amountOfHamon <= 12)
            {
                frame = 0;
            }
            if (hamonPlayer.amountOfHamon >= 13 && hamonPlayer.amountOfHamon <= 24)
            {
                frame = 1;
            }
            if (hamonPlayer.amountOfHamon >= 25 && hamonPlayer.amountOfHamon <= 36)
            {
                frame = 2;
            }
            if (hamonPlayer.amountOfHamon >= 37 && hamonPlayer.amountOfHamon <= 48)
            {
                frame = 3;
            }
            if (hamonPlayer.amountOfHamon >= 49 && hamonPlayer.amountOfHamon <= 60)
            {
                frame = 4;
            }
            if (hamonPlayer.amountOfHamon >= 61 && hamonPlayer.amountOfHamon <= 75)
            {
                frame = 5;
            }
            if (hamonPlayer.amountOfHamon >= 76 && hamonPlayer.amountOfHamon <= 90)
            {
                frame = 6;
            }
            if (hamonPlayer.amountOfHamon >= 91 && hamonPlayer.amountOfHamon <= 105)
            {
                frame = 7;
            }
            if (hamonPlayer.amountOfHamon >= 106 && hamonPlayer.amountOfHamon <= 120)
            {
                frame = 8;
            }
            if (hamonPlayer.amountOfHamon >= 121 && hamonPlayer.amountOfHamon <= 140)
            {
                frame = 9;
            }
            if (hamonPlayer.amountOfHamon >= 141 && hamonPlayer.amountOfHamon <= 160)
            {
                frame = 10;
            }
            if (hamonPlayer.amountOfHamon >= 161 && hamonPlayer.amountOfHamon <= 180)
            {
                frame = 11;
            }
            if (hamonPlayer.amountOfHamon >= 181 && hamonPlayer.amountOfHamon <= 192)   //Aja stone frames
            {
                frame = 12;
            }
            if (hamonPlayer.amountOfHamon >= 193 && hamonPlayer.amountOfHamon <= 204)
            {
                frame = 13;
            }
            if (hamonPlayer.amountOfHamon >= 205 && hamonPlayer.amountOfHamon <= 216)
            {
                frame = 14;
            }
            if (hamonPlayer.amountOfHamon >= 217 && hamonPlayer.amountOfHamon <= 228)
            {
                frame = 15;
            }
            if (hamonPlayer.amountOfHamon >= 229 && hamonPlayer.amountOfHamon <= 240)       //last of the first row
            {
                frame = 16;
            }
            if (hamonPlayer.amountOfHamon >= 241 && hamonPlayer.amountOfHamon <= 255)       //2nd row starts here
            {
                frame = 17;
            }
            if (hamonPlayer.amountOfHamon >= 256 && hamonPlayer.amountOfHamon <= 270)
            {
                frame = 18;
            }
            if (hamonPlayer.amountOfHamon >= 271 && hamonPlayer.amountOfHamon <= 285)
            {
                frame = 19;
            }
            if (hamonPlayer.amountOfHamon >= 286 && hamonPlayer.amountOfHamon <= 300)       //3rd row starts here
            {
                frame = 20;
            }
            if (hamonPlayer.amountOfHamon >= 301 && hamonPlayer.amountOfHamon <= 320)
            {
                frame = 21;
            }
            if (hamonPlayer.amountOfHamon >= 321 && hamonPlayer.amountOfHamon <= 340)
            {
                frame = 22;
            }
            if (hamonPlayer.amountOfHamon >= 341 && hamonPlayer.amountOfHamon <= 360)
            {
                frame = 23;
            }*/

            /*Rectangle clippingRect = hamonBar.GetClippingRectangle(spriteBatch);
            //lostSpace.X = clippingRect.Width - (int)(clippingRect.Width * Main.UIScale);
            Point lostSpace = new Point((int)(clippingRect.Width * (1f / Main.UIScale)), (int)(clippingRect.Height * (1f / Main.UIScale)));
            lostSpace = Point.Zero;
            //lostSpace -= (clippingRect.Size() * (1f / Main.UIScale)).ToPoint();

            Rectangle scaledRect = new Rectangle((int)(clippingRect.X * (1f / Main.UIScale)) + lostSpace.X, (int)(clippingRect.Y * (1f / Main.UIScale)) + lostSpace.Y, (int)(clippingRect.Width * Main.UIScale), (int)(clippingRect.Height * Main.UIScale));
            if (Main.UIScale > 1f)
            {
                scaledRect.X = (int)(clippingRect.X / Main.UIScale) + lostSpace.X;
                scaledRect.Y = (int)(clippingRect.Y / Main.UIScale) + lostSpace.Y;
            }*/
            spriteBatch.Draw(hamonBarTexture, hamonBar.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, hamonBarTexture.Width, frameHeight), Color.Yellow);
        }
    }
}