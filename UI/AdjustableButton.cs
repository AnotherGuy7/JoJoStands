using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class AdjustableButton : UIImageButton
    {
        private Texture2D buttonImage;
        private Vector2 drawPosition;
        private Vector2 defaultSize;        //Meant to be a 
        private Rectangle rectangle;
        private float imageScale;

        public UIElement owner;
        public bool focusedOn = false;      //These are meant to be chanegable
        public float focusScaleAmount = 1.3f;
        public Vector2 buttonSize;
        public Color drawColor = Color.White;
        public float drawAlpha = 0f;
        public bool respondToFocus = false;
        public bool lockedInFocus = false;
        public SpriteEffects effect;
        public float rotation;


        public AdjustableButton(Texture2D texture, Vector2 position, Vector2 size, Color color, bool respondToFocusInput = true, float focusScale = 1.15f) : base(texture)
        {
            buttonImage = texture;
            SetButtonPosiiton(position);
            defaultSize = size;
            drawColor = color;
            imageScale = 1f;
            rotation = 0f;
            effect = SpriteEffects.None;
            focusedOn = false;
            focusScaleAmount = focusScale;
            respondToFocus = respondToFocusInput;
            rectangle = new Rectangle(0, 0, (int)size.X, (int)size.Y);
        }

        public override void OnInitialize()
        {
            SetButtonPosiiton(drawPosition);
            SetButtonSize(defaultSize);
        }

        private void ResetVariables()
        {
            imageScale = 1f;
            drawAlpha = 0.4f;
            focusedOn = false;
            SetVisibility(0f, 0f);      //This is to cover the draws of the normal UI
        }


        public override void Update(GameTime gameTime)
        {
            ResetVariables();
            if (lockedInFocus)
            {
                focusedOn = true;
            }
            if (respondToFocus)
            {
                if (focusedOn || IsMouseHovering)
                {
                    drawAlpha = 1f;
                    imageScale = focusScaleAmount;
                    SetButtonSize(defaultSize * imageScale);
                }
                else
                {
                    SetButtonSize(defaultSize);
                }
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Vector2 origin = new Vector2(buttonImage.Width / 2f, buttonImage.Height / 2f);
            Vector2 ownerPosition = Vector2.Zero;
            if (owner.Top.Pixels != 0f || owner.Left.Pixels != 0f)
            {
                ownerPosition = new Vector2(owner.Left.Pixels, owner.Top.Pixels);
            }
            if (owner.HAlign != 0f || owner.VAlign != 0f)
            {
                float fractionWidth = (float)Main.screenWidth * owner.HAlign;
                float fractionHeight = (float)Main.screenHeight * owner.VAlign;
                ownerPosition = new Vector2(fractionWidth, fractionHeight);
            }

            //I got to this end result through trial and error (Apparently using the Align ones make the origin centered???)
            drawPosition = ownerPosition + new Vector2(Left.Pixels, Top.Pixels) - new Vector2(owner.Width.Pixels / 2f, owner.Height.Pixels / 2f) + (origin * 2f);

            spriteBatch.Draw(buttonImage, drawPosition, rectangle, drawColor * drawAlpha, rotation, origin, imageScale, effect, 0f);
        }

        public new void SetImage(Texture2D texture)
        {
            buttonImage = texture;
        }

        public void SetButtonPosiiton(Vector2 pos)
        {
            Top.Pixels = pos.X;
            Left.Pixels = pos.Y;
        }

        public void SetButtonSize(Vector2 size)
        {
            Width.Pixels = size.X;
            Height.Pixels = size.Y;
            buttonSize = new Vector2(Width.Pixels, Height.Pixels);
        }

        public void ChangeDefaultButtonSize(Vector2 newSize)
        {
            defaultSize = newSize;
        }
    }
}