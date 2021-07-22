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

sampler inputSpaceTexture1 : register(s2);

float Random(float number)
{
    return frac(sin(dot(cos(number), tan(number))));
}

float2 RandomFloat2(float number)
{
    float x = frac(sin(dot(cos(number), tan(number))));
    float y = frac(sin(dot(sin(number), tan(sin(number)))));
    return float2(x, y);
}

float4 PixelShaderFunction(float2 UV : TEXCOORD0) : COLOR0
{
    float4 resultColor = tex2D(uImage0 , UV);
    
    resultColor = float4(0f, 0f, 0f, smoothstep(0.2f, 0.8f, UV.y) * smoothstep(0.2f, 0.8f, UV.y));
    bool coloredPixel = Random(UV.x) <= 0.9f;
    if (coloredPixel)
    {

        bool backgroundPixel = Random(UV.x) <= 0.5f;
        //Yeah I got really lost in what to even do.
    }
    return resultColor;
}


technique Technique1
{
    pass SpaceCircleEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}