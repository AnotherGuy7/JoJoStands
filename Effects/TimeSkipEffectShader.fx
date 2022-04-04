sampler uImage0 : register(s0);     //the screen itself
sampler uImage1 : register(s1);
sampler uImage2;
sampler uImage3;
float3 uColor;
float uOpacity : register(C0);
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;

sampler backStarsImage;
sampler frontStarsImage;

float4 darkPurple = float4(0.258, 0.058, 0.349, 1.0);

float baseRadius = 1.4;     //The amount of clear space
float fadeStrength = 3.7;       //How fast the fade clears out into full view (transparency -> fully there)
//float4 mediumPink = float4(0.451, 0.086, 0.412, 1.0);

/*float Random(float number)
{
    return frac(sin(dot(cos(number), tan(number))));
}

float2 RandomFloat2(float number)
{
    float x = frac(sin(dot(cos(number), tan(number))));
    float y = frac(sin(dot(sin(number), tan(sin(number)))));
    return float2(x, y);
}*/

float4 PixelShaderFunction(float2 UV : TEXCOORD0) : COLOR0
{
    float4 pixelColor = tex2D(uImage0 , UV);
    
    //float backSpaceLerpValue = abs(sin(UV.x + (uTime / 8.0)));      //Very back tint
    //float4 backSpaceCol = lerp(darkPurple / 2.0, mediumPink / 2.0, backSpaceLerpValue);
    float4 backSpaceCol = darkPurple / (4 - (1.3 * (sin((uScreenPosition.y / uScreenPosition.y) + uTime)))); //Why does abs() fail to compile???
    
    float2 backStarsUV = UV + float2((uTime / 76.0) + (uScreenPosition.x / (uScreenResolution.x * 3)), 0.025 * sin(UV.x + uTime / 4.8));
    backStarsUV += sin(UV.x * (uTime / 42.6));
    float backStarsAlpha = sin(uTime / 14.0);
    float4 backStarsCol = float4(tex2D(frontStarsImage, backStarsUV * 24.0).rgb, 0.8);

    
    float2 frontStarsUV = UV + float2((uTime / 46.0) + (uScreenPosition.x / (uScreenResolution.x * 2)), 0.076 * cos(UV.x + (uTime / 2.5)));
    float frontStarsAlpha = sin(uTime / 6.0);        //For some really odd reason I could NOT use this expression directly
    float4 frontStarsCol = float4(tex2D(frontStarsImage, frontStarsUV * 16.0).rgb, 0.0);

    float distanceMultiplier = fadeStrength * uProgress;
    /*if (uProgress < 0.1)
        distanceMultiplier = 3.7 * (uProgress / 0.1);
    else if (uProgress > 0.9)
        distanceMultiplier = 3.7 * ((-uProgress + 1.0) / 0.1);*/
        
        float spaceLayerTransparency = baseRadius - (distance(float2(0.5, 0.5), UV) * distanceMultiplier); //The transparency from the center
    float4 resultSpaceColor = backSpaceCol + backStarsCol + frontStarsCol;
    
    resultSpaceColor = lerp(pixelColor, resultSpaceColor, clamp(1.0 - spaceLayerTransparency, 0.0, 1.0));
    return resultSpaceColor * 0.8;
}

technique Technique1
{
    pass TimeSkipEffectShader
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

/*
Description file detected: 7
* ColorChangeEffect.fx
* GreenEffect.fx
* Greyscale.fx
* RedEffect.fx
* TimeSkipEffectShader.fx
* TimestopEffect.fx
* VoidBarGradient.fx

Start loading description file: ColorChangeEffect.fx ..Done!
Start compiling font..Done!
Start compiling font content file: ColorChangeEffect.xnb ..Done!

Start loading description file: GreenEffect.fx ..Done!
Start compiling font..Done!
Start compiling font content file: GreenEffect.xnb ..Done!

Start loading description file: Greyscale.fx ..Done!
Start compiling font..Done!
Start compiling font content file: Greyscale.xnb ..Done!

Start loading description file: RedEffect.fx ..Done!
Start compiling font..Done!
Start compiling font content file: RedEffect.xnb ..Done!

Start loading description file: TimeSkipEffectShader.fx ..Done!
Start compiling font.
Unhandled Exception: Microsoft.Xna.Framework.Content.Pipeline.InvalidContentException: Errors compiling C:\Users\AnotherGuy\Documents\My Games\Terraria\ModLoader\Mod Sources\JoJoStands\Effects\TimeSkipEffectShader.fx:
(69,23): ID3DXEffectCompiler::CompileEffect: There was an error compiling expression
ID3DXEffectCompiler: Compilation failed
   at Microsoft.Xna.Framework.Content.Pipeline.Processors.EffectProcessor.DoSomethingAwesomeWithErrorsAndWarnings(Boolean success, String errorsAndWarnings, EffectContent input, ContentProcessorContext context)
   at Microsoft.Xna.Framework.Content.Pipeline.Processors.EffectProcessor.Process(EffectContent input, ContentProcessorContext context)
   at DynamicFontGenerator.Generator.CompileEffects()
   at DynamicFontGenerator.Generator.Initialize()
   at Microsoft.Xna.Framework.Game.RunGame(Boolean useBlockingRun)
   at Microsoft.Xna.Framework.Game.Run()
   at DynamicFontGenerator.Generator.Main()

*/