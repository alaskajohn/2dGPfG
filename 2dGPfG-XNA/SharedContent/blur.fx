uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShaderFunction(float2 curCoord: TEXCOORD0) : COLOR
{
	float2 center = {0.5f, 0.5f};
	float maxDistSQR = 0.7071f; //precalulated sqrt(0.5f)

	float2 diff = abs(curCoord - center);
	float distSQR = length(diff);
											
	float blurAmount = (distSQR / maxDistSQR) / 100.0f;

	float2 prevCoord = curCoord;
	prevCoord[0] -= blurAmount;

	float2 nextCoord = curCoord;
	nextCoord[0] += blurAmount;

	float4 color = ((tex2D(ScreenS, curCoord)
				  + tex2D(ScreenS, prevCoord)
				  + tex2D(ScreenS, nextCoord))/3.0f);
		
	return color;
}
technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
