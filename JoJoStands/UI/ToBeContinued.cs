using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace JoJoStands.UI
{
    internal class ToBeContinued : UIState
    {
        public UIImage TBCArrow;
        public static bool Visible;
        public static Texture2D TBCArrowTexture;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            TBCArrow = new UIImage(TBCArrowTexture);
            TBCArrow.Left.Set(80f, 0f);
            TBCArrow.Top.Set(510f, 0f);
            TBCArrow.Width.Set(180f, 0f);
            TBCArrow.Height.Set(300f, 0f);
            TBCArrow.ImageScale = 2f;

            Append(TBCArrow);
            base.OnInitialize();
        }
    }
}