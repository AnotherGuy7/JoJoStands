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
    float4 gamePixelColor = tex2D(uImage0, UV);
    if (gamePixelColor.r == 0.0)
        return gamePixelColor;
    
    float3 newColor = lerp(uColor, uSecondaryColor, Random(float2(UV.x / uOpacity, UV.y * uOpacity)));
    
    return float4(newColor.rgb, 1);
}

technique Technique1
{
    pass MultiColorStaticEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}