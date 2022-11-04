using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class SexPistolsUI : UIState
    {
        public DragableUIPanel sexPistolsUI;
        public static bool Visible;
        public static Texture2D sexPistolsUITexture;

        public override void Update(GameTime gameTime)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();

            if (mPlayer.sexPistolsTier == 0 || !mPlayer.standAutoMode)
                Visible = false;

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            sexPistolsUI = new DragableUIPanel();
            sexPistolsUI.Left.Set(800f, 0f);
            sexPistolsUI.Top.Set(510f, 0f);
            sexPistolsUI.Width.Set(150f, 0f);
            sexPistolsUI.Height.Set(150f, 0f);
            sexPistolsUI.BackgroundColor = new Color(0, 0, 0, 0);
            sexPistolsUI.BorderColor = new Color(0, 0, 0, 0);

            Append(sexPistolsUI);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = 6 - mPlayer.sexPistolsLeft;
            int frameHeight = sexPistolsUITexture.Height / 7;
            spriteBatch.Draw(sexPistolsUITexture, sexPistolsUI.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, sexPistolsUITexture.Width, frameHeight), new Color(255f, 255f, 255f, 255f));
        }
    }
}