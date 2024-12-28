sampler uImage0 : register(s0); //the screen itself
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
float4 uShaderSpecificData;

//const float3 color1 = float3(35 / 255, 15 / 255, 56 / 255);
//const float3 color2 = float3(35 / 255, 15 / 255, 56 / 255);
//const float3 color3 = float3(35 / 255, 15 / 255, 56 / 255);

float Random(float2 p)
{
    float2 K1 = float2(
        23.14069263277926, // e^pi (Gelfond's constant)
        2.665144142690225 // 2^sqrt(2) (Gelfond–Schneider constant)
        );
    return frac(cos(dot(p, K1)) * 12345.6789);
}

float4 PixelShaderFunction(float2 UV : TEXCOORD0) : COLOR0
{
    float spritePixels = 96;
    float pixelStepX = 2.0 * (1.0 / spritePixels);      //pixels to jump / 
    float pixelStepY = 2.0 * (1.0 / spritePixels);
    float2 pixelCoord = float2(pixelStepX * floor(UV.x / pixelStepX), pixelStepY * floor(UV.y / pixelStepY));

    float4 gamePixelColor = tex2D(uImage0, pixelCoord);
    if (gamePixelColor.a <= 0.0)
        return gamePixelColor;
    
    //float4 randomPixelColor = tex2D(uImage0, float2(Random(uOpacity), Random(uOpacity / 2.0)));
    float dist = distance(UV, float2(0.5, 0.5));
    if (dist > 0.4 + (0.1 * (uSaturation / 2.0)))
        return gamePixelColor;
    
    float3 newColor = lerp(gamePixelColor.rgb, uColor, (((dist * 10.0) - 2.0) / 2.0) * uSaturation);
    newColor = lerp(newColor, gamePixelColor.rgb, Random(pixelCoord + float2(uOpacity, uOpacity / 2.0)));
    newColor = lerp(newColor, gamePixelColor.rgb, uOpacity);
    return float4(newColor, 1.0);
}

technique Technique1
{
    pass LayeredColorStaticEffect
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}