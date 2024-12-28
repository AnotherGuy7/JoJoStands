using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class VoidBar : UIState
    {
        public DragableUIPanel voidBarUI;
        public static bool Visible;
        public static Texture2D VoidBarTexture;
        public static Texture2D VoidBarBarOutlineTexture;
        public static Texture2D VoidBarBarTexture;
        private UIText voidText;
        private float currentFillValue = 0f;
        private int timer = 0;

        public override void Update(GameTime gameTime)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            voidText.SetText("Void: " + mPlayer.voidCounter);

            if (mPlayer.creamTier == 0)
                Visible = false;

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            voidBarUI = new DragableUIPanel();
            voidBarUI.Left.Set(Main.screenWidth * 0.9f, 0f);
            voidBarUI.Top.Set(Main.screenHeight * 0.9f, 0f);
            voidBarUI.Width.Set(96f * 2f, 0f);
            voidBarUI.Height.Set(96f * 2f, 0f);
            voidBarUI.BackgroundColor = new Color(0, 0, 0, 0);
            voidBarUI.BorderColor = new Color(0, 0, 0, 0);

            voidText = new UIText("");
            voidText.HAlign = 0.5f;
            voidText.VAlign = 1f;
            voidText.Width.Set(22f, 0f);
            voidText.Height.Set(22f, 0f);
            voidBarUI.Append(voidText);

            Append(voidBarUI);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.voidCounterMax == 0)
                return;

            float normalizedVoidCounter = 1f - ((float)mPlayer.voidCounter / (float)mPlayer.voidCounterMax);
            currentFillValue = MathHelper.Lerp(currentFillValue, normalizedVoidCounter, 0.21f);

            Rectangle mainUIDestinationRect = UITools.ReformatRectangle(voidBarUI.GetClippingRectangle(spriteBatch));
            MiscShaderData layeredStaticEffectShader = JoJoStandsShaders.GetShaderInstance(JoJoStandsShaders.LayeredColorStaticEffect);
            layeredStaticEffectShader.UseOpacity(Main.rand.Next(0, 100 + 1));
            Color[] colors = new Color[4]
            {
                new Color(35, 15, 56),
                new Color(81, 34, 128),
                new Color(93, 69, 195),
                new Color(187, 93, 224)
            };
            float[] strengths = new float[4]
            {
                Main.rand.Next(0, 100 + 1),
                Main.rand.Next(0, 100 + 1) * ((currentFillValue * 0.66f) + 0.33f),
                Main.rand.Next(0, 100 + 1) * ((currentFillValue * 0.33f) + 0.66f),
                Main.rand.Next(0, 100 + 1) * currentFillValue
            };
            //layeredStaticEffectShader.Shader.Parameters["colors"].SetValue(colors);
            //layeredStaticEffectShader.Shader.Parameters["colorStrength"].SetValue(strengths);
            timer += (int)(16 * ((0.8f * (1f - currentFillValue)) + 0.2f));
            if (timer >= 360)
                timer = 0;

            float fluctuation = ((float)System.Math.Sin(MathHelper.ToRadians(timer)) + 1f) / 2f;
            float offset = 0.6f * (1f - currentFillValue);
            layeredStaticEffectShader.UseOpacity(1f - ((fluctuation * (1f - offset)) + offset));       //random
            layeredStaticEffectShader.UseSaturation(1f - currentFillValue);        //progress
            layeredStaticEffectShader.UseColor(colors[(int)((1f - currentFillValue) * 3)]);
            layeredStaticEffectShader.UseSecondaryColor(Color.White);
            UITools.DrawUIWithShader(spriteBatch, layeredStaticEffectShader, VoidBarTexture, mainUIDestinationRect);

            //spriteBatch.Draw(VoidBarTexture, mainUIDestinationRect, new Rectangle(0, 0, VoidBarTexture.Width, VoidBarTexture.Height), Color.White);
            MiscShaderData voidGradientShader = JoJoStandsShaders.GetShaderInstance(JoJoStandsShaders.VoidBarGradient);
            float gradientValue = ((48f / 96f) * currentFillValue) + (24f / 96f);
            voidGradientShader.UseOpacity(gradientValue);

            spriteBatch.Draw(VoidBarBarOutlineTexture, mainUIDestinationRect, Color.White);
            UITools.DrawUIWithShader(spriteBatch, voidGradientShader, VoidBarBarTexture, mainUIDestinationRect);
        }

        /*private void PreDrawVoidCounterGradient(MyPlayer mPlayer, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            MiscShaderData voidGradientShader = GameShaders.Misc["JoJoStandsVoidGradient"];
            voidGradientShader.UseOpacity(mPlayer.voidCounter / mPlayer.voidCounterMax);

            voidGradientShader.Apply(null);
            spriteBatch.Draw(VoidBarBarTexture, voidBarUI.GetClippingRectangle(spriteBatch), new Rectangle(0, 0, VoidBarTexture.Width, VoidBarTexture.Height), Color.White);
        }

        private void PostDrawVoidCounterGradient(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }*/
    }
}