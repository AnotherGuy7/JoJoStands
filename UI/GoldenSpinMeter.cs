using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class GoldenSpinMeter : UIState
    {
        public DragableUIPanel GSpinMeter;
        public static bool Visible;
        public UIText spinAmountText;
        public static Texture2D goldenRectangleTexture;
        public static Texture2D goldenRectangleSpinLineTexture;

        public override void Update(GameTime gameTime)
        {
            MyPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            spinAmountText.SetText("Spin: " + mPlayer.goldenSpinCounter);
            if (mPlayer.goldenSpinCounter <= 0)
                Visible = false;

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            GSpinMeter = new DragableUIPanel();
            GSpinMeter.HAlign = 0.9f;
            GSpinMeter.VAlign = 0.9f;
            GSpinMeter.Width.Set(140f, 0f);
            GSpinMeter.Height.Set(96f, 0f);
            GSpinMeter.BackgroundColor = new Color(0, 0, 0, 0);
            GSpinMeter.BorderColor = new Color(0, 0, 0, 0);

            spinAmountText = new UIText("");
            spinAmountText.HAlign = 0.5f;
            spinAmountText.VAlign = 1.4f;
            spinAmountText.Width.Set(22f, 0f);
            spinAmountText.Height.Set(22f, 0f);
            GSpinMeter.Append(spinAmountText);

            Append(GSpinMeter);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.LocalPlayer;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            float frame = (mPlayer.goldenSpinCounter / 300f) * 12f;
            int frameHeight = goldenRectangleTexture.Height / 12;      //12 frames in that sheet, both sheets are the same height

            spriteBatch.Draw(goldenRectangleTexture, GSpinMeter.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * (int)(frame - 1), goldenRectangleTexture.Width, frameHeight), Color.Yellow);
            spriteBatch.Draw(goldenRectangleSpinLineTexture, GSpinMeter.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * (int)(frame - 1), goldenRectangleSpinLineTexture.Width, frameHeight), Color.Yellow);
        }
    }
}