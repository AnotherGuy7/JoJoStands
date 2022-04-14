using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class HamonBarState : UIState      //ExamplpMod's ExampleUI, CoinPanel. Look for an easier and cleaner way of doig the text thing in the future
    {
        internal static int sizeMode;
        public DragableUIPanel HamonBar;
        public static bool Visible;
        public UIText hamonDisplay;
        public static Texture2D hamonBarTexture;
        public static bool changedInConfig = false;

        public static void ShowHamonBar()
        {
            Visible = true;
        }

        public static void HideHamonBar()
        {
            Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            Items.Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Items.Hamon.HamonPlayer>();
            if (sizeMode >= 4)
            {
                sizeMode = 3;
            }
            if (sizeMode <= 0)
            {
                sizeMode = 0;
                Visible = false;
            }
            if (sizeMode == 1)      //default size
            {
                Visible = true;
                HamonBar.Width.Set(140f, 0f);
                HamonBar.Height.Set(96f, 0f);

                hamonDisplay.Left.Set(28f, 0f);
                hamonDisplay.Top.Set(80f, 0f);
            }
            if (sizeMode == 2)
            {
                Visible = true;
                HamonBar.Width.Set(210f, 0f);
                HamonBar.Height.Set(144f, 0f);

                hamonDisplay.Left.Set(60f, 0f);
                hamonDisplay.Top.Set(120f, 0f);
            }
            if (sizeMode == 3)
            {
                Visible = true;
                HamonBar.Width.Set(280f, 0f);
                HamonBar.Height.Set(192f, 0f);

                hamonDisplay.Left.Set(90f, 0f);
                hamonDisplay.Top.Set(150f, 0f);
            }
            hamonDisplay.SetText(hamonPlayer.amountOfHamon + "/" + hamonPlayer.maxHamon);
            if (changedInConfig)
            {
                HamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
                HamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
                changedInConfig = false;
            }


            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            HamonBar = new DragableUIPanel();
            HamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
            HamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
            HamonBar.Width.Set(140f, 0f);
            HamonBar.Height.Set(96f, 0f);
            HamonBar.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            HamonBar.BorderColor = new Color(0, 0, 0, 0);

            hamonDisplay = new UIText(0 + "/" + 0);
            hamonDisplay.Left.Set(70f, 0f);
            hamonDisplay.Top.Set(150f, 0f);
            hamonDisplay.Width.Set(22f, 0f);
            hamonDisplay.Height.Set(22f, 0f);
            HamonBar.Append(hamonDisplay);

            Append(HamonBar);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.player[Main.myPlayer];
            Items.Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Items.Hamon.HamonPlayer>();
            int frame = 0;
            int frameHeight = hamonBarTexture.Height / 24;      //24 frames in that sheet
            if (hamonPlayer.amountOfHamon >= 3 && hamonPlayer.amountOfHamon <= 12)
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
            }
            spriteBatch.Draw(hamonBarTexture, HamonBar.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, hamonBarTexture.Width, frameHeight), Color.Yellow);
        }
    }
}