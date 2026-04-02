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
            capsuleCounterUI.Left.Set(Main.screenWidth - 156f - 10f, 0f);
            capsuleCounterUI.Top.Set(Main.screenHeight - 156f - 10f, 0f);
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
            int frame = 6 - mPlayer.purpleHazeCapsules;
            animRect.Y = frame * animRect.Height;

            spriteBatch.Draw(CapsuleCounterTexture, UITools.ReformatRectangle(capsuleCounterUI.GetClippingRectangle(spriteBatch)), animRect, Color.White);
        }
    }
}