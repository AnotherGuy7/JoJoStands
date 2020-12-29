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
    internal class VoidBar : UIState
    {
        public DragableUIPanel VoidBarUI;
        public static bool Visible;
        public static Texture2D VoidBarTexture;
        public UIText VoidText;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MyPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            VoidText.SetText("Void: " + mPlayer.VoidCounter);
        }

        public override void OnInitialize()
        {
            VoidBarUI = new DragableUIPanel();
            VoidBarUI.HAlign = 0.9f;
            VoidBarUI.VAlign = 0.9f;
            VoidBarUI.Width.Set(140f, 0f);
            VoidBarUI.Height.Set(96f, 0f);
            VoidBarUI.BackgroundColor = new Color(0, 0, 0, 0);
            VoidBarUI.BorderColor = new Color(0, 0, 0, 0);

            VoidText = new UIText("");
            VoidText.HAlign = 0.5f;
            VoidText.VAlign = 1.4f;
            VoidText.Width.Set(22f, 0f);
            VoidText.Height.Set(22f, 0f);
            VoidBarUI.Append(VoidText);

            Append(VoidBarUI);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = 12 - mPlayer.VoidCounter;
            int frameHeight = VoidBarTexture.Height / 12;
            spriteBatch.Draw(VoidBarTexture, VoidBarUI.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, VoidBarTexture.Width, frameHeight), new Color(255f, 255f, 255f, 255f));
        }
    }
}