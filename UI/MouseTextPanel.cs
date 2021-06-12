using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace JoJoStands.UI
{
    public class MouseTextPanel : UIPanel
    {
        public UIText uiText;
        public Vector2 ownerPos;

        public bool visible = false;

        public MouseTextPanel(int width, int height, string defaultText = "")
        {
            Width.Pixels = width;

            uiText = new UIText(defaultText);
            Append(uiText);
        }

        public void ShowText(string newText)
        {
            visible = true;
            uiText.SetText(newText);
            Height.Pixels = Main.fontMouseText.MeasureString(uiText.Text).Y + 8f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!visible)
                return;

            Left.Pixels = Main.MouseScreen.X + 10f - ownerPos.X;
            Top.Pixels = Main.MouseScreen.Y + 10f - ownerPos.Y;

            uiText.Left.Pixels = 4f;
            uiText.Top.Pixels = 2f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!visible)
                return;

            base.Draw(spriteBatch);
        }
    }
}
