using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace JoJoStands.UI
{
    public class UITools
    {
        /// <summary>
        /// Takes in rectangle and recreates it so that it can match with the zoom matrix.
        /// </summary>
        /// <param name="Rectangle"></param>
        public static Rectangle ReformatRectangle(Rectangle rectangle)
        {
            float scaleInverse = 1f - (Main.UIScale - 1f);
            Rectangle clippingRect = rectangle;
            Point transformedPosition = Vector2.Transform(clippingRect.Location.ToVector2(), Matrix.Invert(Main.UIScaleMatrix)).ToPoint();
            return new Rectangle(transformedPosition.X, transformedPosition.Y, (int)(clippingRect.Width * scaleInverse), (int)(clippingRect.Height * scaleInverse));
        }

        public static void DrawUIWithShader(SpriteBatch spriteBatch, MiscShaderData shaderData, Texture2D texture, Rectangle clippingRectangle)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            shaderData.Apply(null);
            spriteBatch.Draw(texture, clippingRectangle, Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        }
    }
}
