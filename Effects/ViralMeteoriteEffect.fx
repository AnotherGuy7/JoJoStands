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

float4 ViralColor = float4(0.87, 0.62, 0, 1);

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 UV = coords;
    UV.y += (0.0011f * sin((UV.x * 500) + (uTime * 5.0) + uScreenPosition.x / 5.0)) * uProgress;
    float4 pixelColor = tex2D(uImage0, UV);
    
    float2 secondUV = coords;
    secondUV.x += 0.006f * sin((UV.y * 9.0) + (uTime * 2.0)) * uProgress;
    secondUV.x += 0.00005f * tan(UV.y + (uTime * 25.0) * (uProgress * 1.4f) * 0.08f);
    pixelColor += tex2D(uImage0, secondUV) * 0.2f;
    
    float4 newColor = lerp(pixelColor, ViralColor, 0.13 * uProgress);
	return newColor;
}

technique Technique1
{
    pass ViralMeteoriteEffect
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}