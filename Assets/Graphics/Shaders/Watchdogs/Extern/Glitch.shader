// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MyShaders/Glitch" {

	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_dispTex("Base (RGB)", 2D) = "bump" {}
	_intensity("Glitch Intensity", float) = 0.5
		_aberration("Aberration Strength", float) = 0
	}

		CGINCLUDE
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
	uniform sampler2D _dispTex;
	float _intensity;
	float _aberration;
	float flip_up;
	float flip_down;
	float displace;
	float scale;

	struct v2f
	{
		float4 pos    : POSITION;
		float2 uv    : TEXCOORD0;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	half4 frag(v2f i) : COLOR
	{

		float2 ooo = (i.uv - 0.5) * 2.0;
		float2 uv;
		uv.x = (1 - ooo.y * ooo.y) * sqrt(_intensity) *ooo.x;
		uv.y = (1 - ooo.x * ooo.x) * sqrt(_intensity) *ooo.y;
		

		float4 normal = tex2D(_dispTex, uv);


		if (i.uv.y < flip_up)
			i.uv.y = 1 - (uv.y + flip_up);

		if (i.uv.y > flip_down)
			i.uv.y = 1 - (uv.y - flip_down);

		i.uv.xy += (normal.xy - 0.5) *displace * _intensity;



		float2 rectangle = float2(i.uv.x - 0.5, i.uv.y - 0.5);
		float dist = sqrt(pow(rectangle.x, 2) + pow(rectangle.y, 2));

		float mov = _aberration * dist / 2;
		float2 uvR, uvG, uvB = (float2)0;

		uvR = float2(i.uv.x - mov, i.uv.y - mov);
		uvG = float2(i.uv.x + mov, i.uv.y + mov);
		uvB = float2(i.uv.x - abs(mov / 2), i.uv.y + abs(mov / 2));

		float sumR = tex2D(_MainTex, uvR).r;
		float sumG = tex2D(_MainTex, uvG).g;
		float sumB = tex2D(_MainTex, uvB).b;
		return fixed4(sumR, sumG, sumB, 1);
	}

		ENDCG

		SubShader
	{
		Pass
		{
			ZTest Always
			Cull Off
			ZWrite Off

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma shader_feature INVERT
			ENDCG
		}
	}
	Fallback off
}