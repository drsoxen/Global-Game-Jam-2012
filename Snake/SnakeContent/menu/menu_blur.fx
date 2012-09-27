float2   ViewportSize    : register(c0);
float2   TextureSize     : register(c1);
float4x4 MatrixTransform : register(c2);

Texture xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};

#define SAMPLE_SQUARE 4
#define SAMPLES SAMPLE_SQUARE * SAMPLE_SQUARE
float2 offsets[SAMPLES];
float weights[SAMPLES];

void vs(inout float4 position : POSITION0,
		  				inout float4 color    : COLOR0,
						inout float2 texCoord : TEXCOORD0)
{
    position = mul(position, transpose(MatrixTransform));
	position.xy -= 0.5;
	position.xy /= ViewportSize;
	position.xy *= float2(2, -2);
	position.xy -= float2(1, -1);
	color = float4(0,0,0,1);
}

void ps(inout float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	float3 c[SAMPLES];
	int i;
	for(i=0; i < SAMPLES; ++i)
		c[i] = tex2D(TextureSampler, texCoord + offsets[0]).rgb * weights[i];
	for(i=0; i < SAMPLES; ++i)
		color.rgb += c[i];
}


technique SpriteBatch
{
	pass
	{
		VertexShader = compile vs_2_0 vs();
		PixelShader  = compile ps_2_0 ps();
	}
}
