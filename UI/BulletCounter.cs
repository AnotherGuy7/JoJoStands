using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace JoJoStands.UI
{
    internal class BulletCounter : UIState
    {
        public DragableUIPanel bulletCountUI;
        public static bool Visible;
        public static Texture2D bulletCounterTexture;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            bulletCountUI = new DragableUIPanel();
            bulletCountUI.Left.Set(800f, 0f);
            bulletCountUI.Top.Set(510f, 0f);
            bulletCountUI.Width.Set(100f, 0f);
            bulletCountUI.Height.Set(100f, 0f);
            bulletCountUI.BackgroundColor = new Color(0, 0, 0, 0);
            bulletCountUI.BorderColor = new Color(0, 0, 0, 0);

            Append(bulletCountUI);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            int frame = 0;
            int frameHeight = bulletCounterTexture.Height / 7;      //24 frames in that sheet
            if (Items.SexPistolsFinal.bulletCount == 0)
            {
                frame = 0;
            }
            if (Items.SexPistolsFinal.bulletCount == 1)
            {
                frame = 1;
            }
            if (Items.SexPistolsFinal.bulletCount == 2)
            {
                frame = 2;
            }
            if (Items.SexPistolsFinal.bulletCount == 3)
            {
                frame = 3;
            }
            if (Items.SexPistolsFinal.bulletCount == 4)
            {
                frame = 4;
            }
            if (Items.SexPistolsFinal.bulletCount == 5)
            {
                frame = 5;
            }
            if (Items.SexPistolsFinal.bulletCount == 6)
            {
                frame = 6;
            }
            spriteBatch.Draw(bulletCounterTexture, bulletCountUI.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, bulletCounterTexture.Width, frameHeight), new Color(255f, 255f, 255f, 255f));
        }
    }
}