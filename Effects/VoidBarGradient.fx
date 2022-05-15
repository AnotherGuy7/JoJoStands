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
float4 uShaderSpecificData;

float NormalizedColor(float value)
{
	return value / 255.0;
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0 , coords);
	
	if (color.r == 0.0)		//Transparent pixel
		return color;
	
	//float2 uv = coords / uScreenResolution;
    float3 topColor = float3(1.0, 0.0, NormalizedColor(191.0));			//Light pink
    float3 bottomColor = float3(NormalizedColor(76.0), NormalizedColor(11.0), NormalizedColor(92.0));		//Dark purple
	
	//float4 textureColors = tex2D(uImage1, coords.xy);
	
	if (coords.y < uOpacity)
	{
		color.a = 1.0;
	}
	else
	{
		color.rgb = lerp(topColor, bottomColor, coords.y);
	}
	return color;
}


technique Technique1
{
    pass VoidBarGradient
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}