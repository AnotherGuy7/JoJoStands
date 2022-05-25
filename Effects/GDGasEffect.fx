sampler uImage0 : register(s0);     //the screen itself
sampler uImage1 : register(s1);
sampler uImage2;
sampler uImage3;
float3 uColor;
float uOpacity;
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

float4 GDPurple = float4(0.84, 0, 1, 1);

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 UV = coords;
    UV.y += 0.02f * sin((UV.x * 22) + uTime / 2.0) * distance(float2(0.5, 0.5), coords);
    float4 pixelColor = tex2D(uImage0, UV);
    
    float4 newColor = lerp(pixelColor, GDPurple, 0.11);
	return newColor; 
}

technique Technique1
{
    pass GDGasEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}