using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace JoJoStands.UI
{
    public class UITools
    {
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
