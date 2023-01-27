using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class HamonBar : UIState
    {
        public static bool visible;
        public static int sizeMode;

        public DragableUIPanel hamonBar;
        public UIText hamonDisplay;
        public static Texture2D hamonBarTexture;
        public static bool changedInConfig = false;

        private static Rectangle animRect;

        public static void ShowHamonBar()
        {
            visible = true;
        }

        public static void HideHamonBar()
        {
            visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (!visible)
            {
                base.Update(gameTime);
                return;
            }

            if (sizeMode >= 4)
                sizeMode = 3;
            if (sizeMode <= 0)
            {
                sizeMode = 0;
                visible = false;
            }

            hamonBar.Width.Set(70f + (70 * sizeMode), 0f);
            hamonBar.Height.Set(48f + (48 * sizeMode), 0f);
            hamonDisplay.Left.Set(5f + (30 * sizeMode) * (1f - (Main.UIScale - 1f)), 0f);
            hamonDisplay.Top.Set(30f + (40 * sizeMode) * (1f - (Main.UIScale - 1f)), 0f);

            hamonDisplay.SetText(hamonPlayer.amountOfHamon + "/" + hamonPlayer.maxHamon);
            if (changedInConfig)
            {
                hamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
                hamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
                changedInConfig = false;
            }


            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            hamonBar = new DragableUIPanel();
            hamonBar.Left.Set(MyPlayer.HamonBarPositionX * (Main.screenWidth * 0.01f), 0f);
            hamonBar.Top.Set(MyPlayer.HamonBarPositionY * (Main.screenHeight * 0.01f), 0f);
            hamonBar.Width.Set(140f, 0f);
            hamonBar.Height.Set(96f, 0f);
            hamonBar.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            hamonBar.BorderColor = new Color(0, 0, 0, 0);

            hamonDisplay = new UIText(0 + "/" + 0);
            hamonDisplay.Left.Set(70f, 0f);
            hamonDisplay.Top.Set(150f, 0f);
            hamonDisplay.Width.Set(22f, 0f);
            hamonDisplay.Height.Set(22f, 0f);
            hamonBar.Append(hamonDisplay);

            Append(hamonBar);
            int frameHeight = hamonBarTexture.Height / 24;
            animRect = new Rectangle(0, 0, hamonBarTexture.Width, frameHeight);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            int frame = (int)MathHelper.Clamp(hamonPlayer.amountOfHamon / 12, 1, 23) - 1;
            animRect.Y = frame * animRect.Height;

            float scaleInverse = 1f - (Main.UIScale - 1f);
            Rectangle clippingRect = hamonBar.GetClippingRectangle(spriteBatch);
            Point transformedPosition = Vector2.Transform(clippingRect.Location.ToVector2(), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
            Rectangle destinationRect = new Rectangle(transformedPosition.X, transformedPosition.Y, (int)(clippingRect.Width * scaleInverse), (int)(clippingRect.Height * scaleInverse));
            spriteBatch.Draw(hamonBarTexture, destinationRect, animRect, Color.Yellow);
        }
    }
}