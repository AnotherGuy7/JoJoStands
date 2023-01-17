using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class BulletCounter : UIState
    {
        public DragableUIPanel bulletCountUI;
        public static bool Visible;
        public static Texture2D bulletCounterTexture;

        private Rectangle animRect;

        public override void OnInitialize()
        {
            bulletCountUI = new DragableUIPanel();
            bulletCountUI.Left.Set(800f, 0f);
            bulletCountUI.Top.Set(510f, 0f);
            bulletCountUI.Width.Set(150f, 0f);
            bulletCountUI.Height.Set(150f, 0f);
            bulletCountUI.BackgroundColor = new Color(0, 0, 0, 0);
            bulletCountUI.BorderColor = new Color(0, 0, 0, 0);

            Append(bulletCountUI);
            int frameHeight = bulletCounterTexture.Height / 7;
            animRect = new Rectangle(0, 0, bulletCounterTexture.Width, frameHeight);

            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = mPlayer.revolverBulletsShot;
            animRect.Y = frame;

            float scaleInverse = 1f - (Main.UIScale - 1f);
            Rectangle clippingRect = bulletCountUI.GetClippingRectangle(spriteBatch);
            Point transformedPosition = Vector2.Transform(clippingRect.Location.ToVector2(), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
            Rectangle mainUIdestinationRect = new Rectangle(transformedPosition.X, transformedPosition.Y, (int)(clippingRect.Width * scaleInverse), (int)(clippingRect.Height * scaleInverse));
            spriteBatch.Draw(bulletCounterTexture, mainUIdestinationRect, animRect, Color.White);
        }
    }
}