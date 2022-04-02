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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0 , coords);

    if (uProgress == 1)
    {
        color.rgb = color.bgb;
    }
    else if (uProgress == 2)
    {
        color.rgb = color.rbg;
    }
    else if (uProgress == 3)
    {
        color.rgb = color.brg;
    }
    else if (uProgress == 4)
    {
        color.rgb = color.grb;
    }
    else if (uProgress == 5)
    {
        color.rgb = color.rrg;
    }
    
    return color;
}


technique Technique1
{
    pass ColorChangeEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}