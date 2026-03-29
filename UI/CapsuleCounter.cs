using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class CapsuleCounter : UIState
    {
        public DragableUIPanel capsuleCounterUI;
        public static bool Visible;
        public static Texture2D CapsuleCounterTexture;

        private Rectangle animRect;

        public override void OnInitialize()
        {
            capsuleCounterUI = new DragableUIPanel();
            capsuleCounterUI.Left.Set(800f, 0f);
            capsuleCounterUI.Top.Set(510f, 0f);
            capsuleCounterUI.Width.Set(156f, 0f);
            capsuleCounterUI.Height.Set(156f, 0f);
            capsuleCounterUI.BackgroundColor = new Color(0, 0, 0, 0);
            capsuleCounterUI.BorderColor = new Color(0, 0, 0, 0);

            Append(capsuleCounterUI);
            int frameHeight = CapsuleCounterTexture.Height / 7;
            animRect = new Rectangle(0, 0, CapsuleCounterTexture.Width, frameHeight);

            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = 7 - mPlayer.purpleHazeCapsules;
            if (frame >= 7)
                frame = 6;
            animRect.Y = frame * animRect.Height;

            spriteBatch.Draw(CapsuleCounterTexture, UITools.ReformatRectangle(capsuleCounterUI.GetClippingRectangle(spriteBatch)), animRect, Color.White);
        }
    }
}