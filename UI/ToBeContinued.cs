using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class ToBeContinued : UIState
    {
        public static bool Visible;
        public static Texture2D TBCArrowTexture;

        private UIImage toBeContinuedArrow;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            toBeContinuedArrow = new UIImage(TBCArrowTexture);
            toBeContinuedArrow.Width.Set(280f, 0f);
            toBeContinuedArrow.Height.Set(52f, 0f);
            toBeContinuedArrow.ImageScale = 1.5f;
            toBeContinuedArrow.HAlign = 0.06f;
            toBeContinuedArrow.VAlign = 0.97f;
            //toBeContinuedArrow.ImageScale = 2f;

            Append(toBeContinuedArrow);
        }
    }
}