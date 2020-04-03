using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace JoJoStands.UI
{
    internal class HamonBarState : UIState      //ExamplpMod's ExampleUI, CoinPanel. Look for an easier and cleaner way of doig the text thing in the future
    {
        internal static int sizeMode;
        public DragableUIPanel HamonBar;
        public static bool Visible;
        public UIText hamonDisplay;
        public static Texture2D hamonBarTexture;

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
            hamonDisplay.SetText(hamonPlayer.HamonCounter + "/" + hamonPlayer.maxHamon);

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            HamonBar = new DragableUIPanel();
            HamonBar.Left.Set(MyPlayer.HamonBarPositionX, 0f);
            HamonBar.Top.Set(MyPlayer.HamonBarPositionY, 0f);
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

        public override void RightClick(UIMouseEvent evt)
        {
            Visible = false;
            base.RightClick(evt);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.player[Main.myPlayer];
            Items.Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Items.Hamon.HamonPlayer>();
            int frame = 0;
            int frameHeight = hamonBarTexture.Height / 24;      //24 frames in that sheet
            if (hamonPlayer.HamonCounter >= 3 && hamonPlayer.HamonCounter <= 12)
            {
                frame = 0;
            }
            if (hamonPlayer.HamonCounter >= 13 && hamonPlayer.HamonCounter <= 24)
            {
                frame = 1;
            }
            if (hamonPlayer.HamonCounter >= 25 && hamonPlayer.HamonCounter <= 36)
            {
                frame = 2;
            }
            if (hamonPlayer.HamonCounter >= 37 && hamonPlayer.HamonCounter <= 48)
            {
                frame = 3;
            }
            if (hamonPlayer.HamonCounter >= 49 && hamonPlayer.HamonCounter <= 60)
            {
                frame = 4;
            }
            if (hamonPlayer.HamonCounter >= 61 && hamonPlayer.HamonCounter <= 75)
            {
                frame = 5;
            }
            if (hamonPlayer.HamonCounter >= 76 && hamonPlayer.HamonCounter <= 90)
            {
                frame = 6;
            }
            if (hamonPlayer.HamonCounter >= 91 && hamonPlayer.HamonCounter <= 105)
            {
                frame = 7;
            }
            if (hamonPlayer.HamonCounter >= 106 && hamonPlayer.HamonCounter <= 120)
            {
                frame = 8;
            }
            if (hamonPlayer.HamonCounter >= 121 && hamonPlayer.HamonCounter <= 140)
            {
                frame = 9;
            }
            if (hamonPlayer.HamonCounter >= 141 && hamonPlayer.HamonCounter <= 160)
            {
                frame = 10;
            }
            if (hamonPlayer.HamonCounter >= 161 && hamonPlayer.HamonCounter <= 180)
            {
                frame = 11;
            }
            if (hamonPlayer.HamonCounter >= 181 && hamonPlayer.HamonCounter <= 192)   //Aja stone frames
            {
                frame = 12;
            }
            if (hamonPlayer.HamonCounter >= 193 && hamonPlayer.HamonCounter <= 204)
            {
                frame = 13;
            }
            if (hamonPlayer.HamonCounter >= 205 && hamonPlayer.HamonCounter <= 216)
            {
                frame = 14;
            }
            if (hamonPlayer.HamonCounter >= 217 && hamonPlayer.HamonCounter <= 228)
            {
                frame = 15;
            }
            if (hamonPlayer.HamonCounter >= 229 && hamonPlayer.HamonCounter <= 240)       //last of the first row
            {
                frame = 16;
            }
            if (hamonPlayer.HamonCounter >= 241 && hamonPlayer.HamonCounter <= 255)       //2nd row starts here
            {
                frame = 17;
            }
            if (hamonPlayer.HamonCounter >= 256 && hamonPlayer.HamonCounter <= 270)
            {
                frame = 18;
            }
            if (hamonPlayer.HamonCounter >= 271 && hamonPlayer.HamonCounter <= 285)
            {
                frame = 19;
            }
            if (hamonPlayer.HamonCounter >= 286 && hamonPlayer.HamonCounter <= 300)       //3rd row starts here
            {
                frame = 20;
            }
            if (hamonPlayer.HamonCounter >= 301 && hamonPlayer.HamonCounter <= 320)
            {
                frame = 21;
            }
            if (hamonPlayer.HamonCounter >= 321 && hamonPlayer.HamonCounter <= 340)
            {
                frame = 22;
            }
            if (hamonPlayer.HamonCounter >= 341 && hamonPlayer.HamonCounter <= 360)
            {
                frame = 23;
            }
            spriteBatch.Draw(hamonBarTexture, HamonBar.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, hamonBarTexture.Width, frameHeight),  Color.Yellow);
        }
    }
}