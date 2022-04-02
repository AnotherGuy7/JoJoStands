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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 Color = tex2D(uImage0, coords);
	Color.g = Color.g - 0.25;
	Color.b = Color.b - 0.25;
	return Color; 
}

technique Technique1
{
    pass RedEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}