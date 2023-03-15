using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class GoldenSpinMeter : UIState
    {
        public DragableUIPanel spinMeter;
        public static bool Visible;
        public UIText spinAmountText;
        public static Texture2D goldenRectangleTexture;
        public static Texture2D goldenRectangleSpinLineTexture;

        private Rectangle animRect;

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
            spinMeter = new DragableUIPanel();
            spinMeter.HAlign = 0.9f;
            spinMeter.VAlign = 0.9f;
            spinMeter.Width.Set(140f, 0f);
            spinMeter.Height.Set(96f, 0f);
            spinMeter.BackgroundColor = new Color(0, 0, 0, 0);
            spinMeter.BorderColor = new Color(0, 0, 0, 0);

            spinAmountText = new UIText("");
            spinAmountText.HAlign = 0.5f;
            spinAmountText.VAlign = 1.4f;
            spinAmountText.Width.Set(22f, 0f);
            spinAmountText.Height.Set(22f, 0f);
            spinMeter.Append(spinAmountText);

            Append(spinMeter);
            int frameHeight = goldenRectangleTexture.Height / 12;      //12 frames in that sheet, both sheets are the same height
            animRect = new Rectangle(0, 0, goldenRectangleTexture.Width, frameHeight);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.LocalPlayer;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = (int)(mPlayer.goldenSpinCounter / 300f) * 12;
            frame += 1;
            if (frame >= 12)
                frame = 11;

            animRect.Y = frame * animRect.Height;

            float scaleInverse = 1f - (Main.UIScale - 1f);
            Rectangle clippingRect = spinMeter.GetClippingRectangle(spriteBatch);
            Point transformedPosition = Vector2.Transform(clippingRect.Location.ToVector2(), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
            Rectangle mainUIdestinationRect = new Rectangle(transformedPosition.X, transformedPosition.Y, (int)(clippingRect.Width * scaleInverse), (int)(clippingRect.Height * scaleInverse));
            spriteBatch.Draw(goldenRectangleTexture, mainUIdestinationRect, animRect, Color.Wheat);
            spriteBatch.Draw(goldenRectangleSpinLineTexture, mainUIdestinationRect, animRect, Color.Wheat);
        }
    }
}