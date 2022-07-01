using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class AdjustableButton : UIImageButton
    {
        private Texture2D buttonImage;
        private Texture2D overlayImage;
        private Vector2 drawPosition;
        private Vector2 defaultSize;        //Meant to be a 
        private Vector2 textureSize;
        private Vector2 overlaySize;
        private Rectangle rectangle;
        private Rectangle clickRect;
        private float imageScale;
        private float overlayScaleReduction;
        private float defaultAlpha = 0.4f;
        private float activeAlpha = 1f;

        public UIElement owner;
        public bool focusedOn = false;      //These are meant to be chanegable
        public float focusScaleAmount = 1.15f;
        public Vector2 buttonPosition;
        public Vector2 screenPosition;
        public Vector2 buttonCenter;
        public Vector2 buttonSize;
        public Vector2 origin;
        public Color drawColor = Color.White;
        public float drawAlpha = 0f;
        public bool respondToFocus = false;
        public bool lockedInFocus = false;
        public bool invisible = false;
        public SpriteEffects effect;
        public float rotation;


        public AdjustableButton(Asset<Texture2D> texture, Vector2 position, Vector2 size, Color color, bool respondToFocusInput, float focusScale = 1.15f) : base(texture)
        {
            buttonImage = (Texture2D)texture;
            SetButtonPosiiton(position);
            defaultSize = size;
            textureSize = texture.Size();
            drawColor = color;
            imageScale = 1f;
            rotation = 0f;
            effect = SpriteEffects.None;
            focusedOn = false;
            focusScaleAmount = focusScale;
            respondToFocus = respondToFocusInput;
            rectangle = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            clickRect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public AdjustableButton(Asset<Texture2D> texture, Vector2 position, Vector2 size, Color color, float defaultAlpha = 0.4f, float activeAlpha = 1f, bool respondToFocusInput = true, float focusScale = 1.15f) : base(texture)
        {
            buttonImage = (Texture2D)texture;
            SetButtonPosiiton(position);
            defaultSize = size;
            textureSize = ((Texture2D)texture).Size();
            drawColor = color;
            imageScale = 1f;
            rotation = 0f;
            effect = SpriteEffects.None;
            focusedOn = false;
            focusScaleAmount = focusScale;
            respondToFocus = respondToFocusInput;
            rectangle = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            clickRect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.defaultAlpha = defaultAlpha;
            this.activeAlpha = activeAlpha;
        }

        public override void OnInitialize()
        {
            SetButtonPosiiton(drawPosition);
            SetButtonSize(defaultSize);
        }

        private void ResetVariables()
        {
            imageScale = 1f;
            if (!invisible)
            {
                drawAlpha = defaultAlpha;
            }
            focusedOn = false;
            SetVisibility(0f, 0f);      //This is to cover the draws of the normal UI
        }


        public override void Update(GameTime gameTime)
        {
            ResetVariables();
            if (!invisible)
            {
                if (lockedInFocus)
                {
                    focusedOn = true;
                }
                if (respondToFocus)
                {
                    if (focusedOn || IsMouseHovering)
                    {
                        drawAlpha = activeAlpha;
                        imageScale = focusScaleAmount;
                        SetButtonSize(defaultSize * imageScale);
                    }
                    else
                    {
                        SetButtonSize(defaultSize);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (invisible)
                return;

            origin = textureSize / 2f;
            Vector2 ownerPosition = Vector2.Zero;

            if (owner != null)
            {
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
                drawPosition = ownerPosition + buttonPosition - new Vector2(owner.Width.Pixels / 2f, owner.Height.Pixels / 2f) + (origin * 2f);
            }
            else
                drawPosition = buttonPosition;

            screenPosition = drawPosition;


            spriteBatch.Draw(buttonImage, drawPosition, rectangle, drawColor * drawAlpha, rotation, origin, imageScale, effect, 0f);

            if (overlayImage != null)
            {
                Vector2 overlayOrigin = overlaySize / 2f;

                spriteBatch.Draw(overlayImage, drawPosition, rectangle, drawColor * drawAlpha, rotation, overlayOrigin, imageScale - overlayScaleReduction, effect, 0f);
            }
        }

        /// <summary>
        /// Sets the default texture for the button.
        /// </summary>
        /// <param name="texture">The texture the button will have.</param>
        public void SetImage(Texture2D texture)
        {
            buttonImage = texture;
        }

        /// <summary>
        /// Gives the button an overlay texture. *Note: The overlay will not have reduced alpha and will remain identical in draw parameters to the original button.
        /// </summary>
        /// <param name="texture">The texture the overlay will use.</param>
        /// <param name="scaleReduction">The amount that the scale will be reduced by.</param>
        public void SetOverlayImage(Texture2D texture, float scaleReduction)
        {
            overlayImage = texture;
            overlayScaleReduction = scaleReduction;
            overlaySize = overlayImage.Size();
        }

        /// <summary>
        /// Sets the button's position in Left and Top.
        /// </summary>
        /// <param name="pos"></param>
        public void SetButtonPosiiton(Vector2 pos)
        {
            Top.Pixels = pos.Y;
            Left.Pixels = pos.X;
            buttonPosition = new Vector2(Left.Pixels, Top.Pixels);
            buttonCenter = buttonPosition + (buttonSize / 2f);
            clickRect.X = (int)(buttonPosition.X - origin.X);
            clickRect.Y = (int)(buttonPosition.Y - origin.Y);

            /*if (owner != null)
            {
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
                screenPosition = ownerPosition + buttonPosition;
            }*/
        }

        /// <summary>
        /// Updates the button's position. Used when object positions are changed without using SetButtonPosition()
        /// </summary>
        public void UpdateButtonPosition()
        {
            buttonPosition = new Vector2(Left.Pixels, Top.Pixels);
            buttonCenter = buttonPosition + (buttonSize / 2f);
            clickRect.X = (int)(buttonPosition.X - origin.X);
            clickRect.Y = (int)(buttonPosition.Y - origin.Y);
        }

        /// <summary>
        /// Checks whether or not the button is being hovered over using a custom rectangle.
        /// </summary>
        /// <returns>Whether or not the button is being hovered over with the cursor.</returns>
        public bool IsButtonHoveredOver()
        {
            return clickRect.Contains(Main.MouseScreen.ToPoint());
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