using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using static Terraria.ModLoader.ModContent;


namespace JoJoStands
{
    public class JoJoStandsShaders
    {
        public const string TimestopEffect = "TimestopEffectShader";
        public const string TimestopGreyscaleEffect = "GreyscaleEffect";
        public const string TimestkipEffect = "TimeSkipEffectShader";
        public const string EpitaphRedEffect = "RedEffect";
        public const string BtZGreenEffect = "GreenEffect";
        public const string BiteTheDustEffect = "BiteTheDustEffect";
        public const string GratefulDeadGasEffect = "GasEffect";
        public const string VoidBarGradient = "JoJoStandsVoidGradient";
        public const string BattlePaletteSwitchEffect = "ColorChangeEffect";
        public const string MultiColorStaticEffect = "MultiColorStaticEffect";
        public const string ViralMeteoriteEffect = "ViralMeteoriteEffect";


        public static void ActivateShader(string shaderName)
        {
            if (Main.dedServ)
                return;

            if (!Filters.Scene[shaderName].IsActive())
                Filters.Scene.Activate(shaderName);
        }

        public static void DeactivateShader(string shaderName)
        {
            if (Main.dedServ)
                return;

            if (Filters.Scene[shaderName].IsActive())
                Filters.Scene[shaderName].Deactivate();
        }

        public static void ChangeShaderActiveState(string shaderName, bool active)
        {
            if (ShaderActive(shaderName) == active)
                return;

            if (active)
                ActivateShader(shaderName);
            else
                DeactivateShader(shaderName);
        }

        public static bool ShaderActive(string shaderName)
        {
            if (Main.dedServ)
                return false;

            return Filters.Scene[shaderName].IsActive();
        }

        public static void ChangeShaderUseProgress(string shaderName, float useProgress)
        {
            if (Main.dedServ)
                return;

            Filters.Scene[shaderName].GetShader().UseProgress(useProgress);
        }

        public static MiscShaderData GetShaderInstance(string shaderName) => GameShaders.Misc[shaderName];

        public static void LoadShaders()
        {
            //Screen shaders
            Ref<Effect> timestopShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/TimestopEffect", AssetRequestMode.ImmediateLoad));      // The path to the compiled shader file.
            Filters.Scene[TimestopEffect] = new Filter(new ScreenShaderData(timestopShader, "TimestopEffectShader"), EffectPriority.VeryHigh);
            Filters.Scene[TimestopEffect].Load();
            Ref<Effect> greyscaleShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/Greyscale", AssetRequestMode.ImmediateLoad));
            Filters.Scene[TimestopGreyscaleEffect] = new Filter(new ScreenShaderData(greyscaleShader, "GreyscaleEffect"), EffectPriority.VeryHigh);
            Filters.Scene[TimestopGreyscaleEffect].Load();
            Ref<Effect> greenShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/GreenEffect", AssetRequestMode.ImmediateLoad));
            Filters.Scene[BtZGreenEffect] = new Filter(new ScreenShaderData(greenShader, "GreenEffect"), EffectPriority.VeryHigh);
            Filters.Scene[BtZGreenEffect].Load();
            Ref<Effect> redShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/RedEffect", AssetRequestMode.ImmediateLoad));
            Filters.Scene[EpitaphRedEffect] = new Filter(new ScreenShaderData(redShader, "RedEffect"), EffectPriority.VeryHigh);
            Filters.Scene[EpitaphRedEffect].Load();
            Ref<Effect> gasShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/GDGasEffect", AssetRequestMode.ImmediateLoad));
            Filters.Scene[GratefulDeadGasEffect] = new Filter(new ScreenShaderData(gasShader, "GDGasEffect"), EffectPriority.VeryHigh);
            Filters.Scene[GratefulDeadGasEffect].Load();
            Ref<Effect> colorChangeShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/ColorChangeEffect", AssetRequestMode.ImmediateLoad));
            Filters.Scene[BattlePaletteSwitchEffect] = new Filter(new ScreenShaderData(colorChangeShader, "ColorChangeEffect"), EffectPriority.VeryHigh);
            Filters.Scene[BattlePaletteSwitchEffect].Load();
            Ref<Effect> viralMeteoriteShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/ViralMeteoriteEffect", AssetRequestMode.ImmediateLoad));
            Filters.Scene[ViralMeteoriteEffect] = new Filter(new ScreenShaderData(viralMeteoriteShader, "ViralMeteoriteEffect"), EffectPriority.VeryHigh);
            Filters.Scene[ViralMeteoriteEffect].Load();

            //Texture shaders
            Ref<Effect> voidGradientShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/VoidBarGradient", AssetRequestMode.ImmediateLoad));
            GameShaders.Misc[VoidBarGradient] = new MiscShaderData(voidGradientShader, "VoidBarGradient");
            Ref<Effect> multiColorStaticEffect = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/MultiColorStaticEffect", AssetRequestMode.ImmediateLoad));
            GameShaders.Misc[MultiColorStaticEffect] = new MiscShaderData(multiColorStaticEffect, "MultiColorStaticEffect");

            Effect timeskipShaderEffect = (Effect)Request<Effect>("JoJoStands/Effects/TimeSkipEffectShader", AssetRequestMode.ImmediateLoad);
            Ref<Effect> timeskipShader = new Ref<Effect>(timeskipShaderEffect);      // The path to the compiled shader file.
            Filters.Scene[TimestkipEffect] = new Filter(new ScreenShaderData(timeskipShader, "TimeSkipEffectShader"), EffectPriority.VeryHigh);
            Filters.Scene[TimestkipEffect].GetShader().UseImage((Texture2D)Request<Texture2D>("JoJoStands/Extras/KingCrimsonBackStars", AssetRequestMode.ImmediateLoad), 0);
            Filters.Scene[TimestkipEffect].GetShader().UseImage((Texture2D)Request<Texture2D>("JoJoStands/Extras/KingCrimsonFrontStars", AssetRequestMode.ImmediateLoad), 1);
            Filters.Scene[TimestkipEffect].Load();

            Ref<Effect> biteTheDustEffectShader = new Ref<Effect>((Effect)Request<Effect>("JoJoStands/Effects/BiteTheDustEffectShader", AssetRequestMode.ImmediateLoad));
            Filters.Scene[BiteTheDustEffect] = new Filter(new ScreenShaderData(biteTheDustEffectShader, "BiteTheDustEffectShader"), EffectPriority.VeryHigh);
            Filters.Scene[BiteTheDustEffect].GetShader().UseImage((Texture2D)Request<Texture2D>("JoJoStands/Extras/KillerQueenBTDImage", AssetRequestMode.ImmediateLoad), 0);
            Filters.Scene[BiteTheDustEffect].Load();
        }
    }
}
