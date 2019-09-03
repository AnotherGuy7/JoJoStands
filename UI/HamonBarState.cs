using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
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
            hamonDisplay.SetText(Main.LocalPlayer.GetModPlayer<MyPlayer>().HamonCounter + "/" + Main.LocalPlayer.GetModPlayer<MyPlayer>().maxHamon);
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            HamonBar = new DragableUIPanel();
            HamonBar.Left.Set(400f, 0f);
            HamonBar.Top.Set(100f, 0f);
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
            Player player = Main.LocalPlayer;
            int frame = 0;
            int frameHeight = hamonBarTexture.Height / 24;      //24 frames in that sheet
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 3 && player.GetModPlayer<MyPlayer>().HamonCounter <= 12)
            {
                frame = 0;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 13 && player.GetModPlayer<MyPlayer>().HamonCounter <= 24)
            {
                frame = 1;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 25 && player.GetModPlayer<MyPlayer>().HamonCounter <= 36)
            {
                frame = 2;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 37 && player.GetModPlayer<MyPlayer>().HamonCounter <= 48)
            {
                frame = 3;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 49 && player.GetModPlayer<MyPlayer>().HamonCounter <= 60)
            {
                frame = 4;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 61 && player.GetModPlayer<MyPlayer>().HamonCounter <= 75)
            {
                frame = 5;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 76 && player.GetModPlayer<MyPlayer>().HamonCounter <= 90)
            {
                frame = 6;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 91 && player.GetModPlayer<MyPlayer>().HamonCounter <= 105)
            {
                frame = 7;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 106 && player.GetModPlayer<MyPlayer>().HamonCounter <= 120)
            {
                frame = 8;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 121 && player.GetModPlayer<MyPlayer>().HamonCounter <= 140)
            {
                frame = 9;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 141 && player.GetModPlayer<MyPlayer>().HamonCounter <= 160)
            {
                frame = 10;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 161 && player.GetModPlayer<MyPlayer>().HamonCounter <= 180)
            {
                frame = 11;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 181 && player.GetModPlayer<MyPlayer>().HamonCounter <= 192)   //Aja stone frames
            {
                frame = 12;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 193 && player.GetModPlayer<MyPlayer>().HamonCounter <= 204)
            {
                frame = 13;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 205 && player.GetModPlayer<MyPlayer>().HamonCounter <= 216)
            {
                frame = 14;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 217 && player.GetModPlayer<MyPlayer>().HamonCounter <= 228)
            {
                frame = 15;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 229 && player.GetModPlayer<MyPlayer>().HamonCounter <= 240)       //last of the first row
            {
                frame = 16;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 241 && player.GetModPlayer<MyPlayer>().HamonCounter <= 255)       //2nd row starts here
            {
                frame = 17;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 256 && player.GetModPlayer<MyPlayer>().HamonCounter <= 270)
            {
                frame = 18;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 271 && player.GetModPlayer<MyPlayer>().HamonCounter <= 285)
            {
                frame = 19;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 286 && player.GetModPlayer<MyPlayer>().HamonCounter <= 300)       //3rd row starts here
            {
                frame = 20;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 301 && player.GetModPlayer<MyPlayer>().HamonCounter <= 320)
            {
                frame = 21;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 321 && player.GetModPlayer<MyPlayer>().HamonCounter <= 340)
            {
                frame = 22;
            }
            if (player.GetModPlayer<MyPlayer>().HamonCounter >= 341 && player.GetModPlayer<MyPlayer>().HamonCounter <= 360)
            {
                frame = 23;
            }
            spriteBatch.Draw(hamonBarTexture, HamonBar.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, hamonBarTexture.Width, frameHeight),  Color.Yellow);
        }
    }
}