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

sampler killerQueenImage;       //No maps?

float4 black = float4(0.0, 0.0, 0.0, 1.0);
//float4 lightPink = float4(1.0, 0.0, 1.0, 1.0);
//float4 yellow = float4(1.0, 1.0, 0.298, 1.0);


float4 PixelShaderFunction(float2 UV : TEXCOORD0) : COLOR0
{
    if (uProgress >= 1.0)
        return black;
    
    float2 distortedUV = float2(cos(UV.x * 6), sin(UV.y * 6));
    distortedUV.y *= -1;
    float4 pixelColor = tex2D(uImage0, distortedUV * (1.0 - (uProgress * 0.7)));
    
    if (distance(UV, float2(0.5, 0.5)) > 0.5 - (uProgress / 2.0))
    {
        float4 outerColor = lerp(pixelColor, black, distance(UV, float2(0.5, 0.5)) * uProgress * 4);
        //outerColor = lerp(outerColor, darkPurple, clamp(sin(UV.x) + frac(cos(UV.y * uTime * 19.0) * UV.x), 0.0, 1.0));
        if (uProgress > 0.5)
        {
            float4 killerQueenColor = tex2D(uImage1, UV);
            outerColor += killerQueenColor * ((uProgress * 2) - 1.6);
        }
        if (uProgress > 0.97)
            outerColor *= uProgress - 1.0;
        return outerColor;
    }
    //float4 areaColor = lerp(lightPink, yellow, tan(uTime * 3.0));
    
    //float pixelColorStrength = (pixelColor.r + pixelColor.g + pixelColor.b) / 3.0;
    //float lerpValue = pixelColorStrength * 0.6;
    
    //float4 resultColor = lerp(pixelColor, areaColor, uProgress * 0.4);
    pixelColor = tex2D(uImage0, UV * (1.0 - uProgress) + (uProgress * float2(0.5, 0.5)));
    float4 resultColor = pixelColor;
    float randomValue = cos(uProgress * (uTime / 24.0));
    if (randomValue <= 0.3)
    {
        resultColor.rgb = pixelColor.gbr;
    }
    else if (randomValue > 0.3 && randomValue <= 0.6)
    {
        resultColor.rgb = pixelColor.rgr;
    }
    else
    {
        resultColor.rgb = pixelColor.brg;
    }
    resultColor *= (2 - (uProgress * 2));
    resultColor = lerp(resultColor, pixelColor, 1.0 - uProgress);
    return resultColor;
}

technique Technique1
{
    pass BiteTheDustEffectShader
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}