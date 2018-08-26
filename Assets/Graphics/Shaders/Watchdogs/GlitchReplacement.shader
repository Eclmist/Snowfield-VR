Shader "Unlit/GlitchReplacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_Fractal("Fractal Noise Mask", 2D) = "white" {}
		_GlitchSpeed("Speed", Range(0,20)) = 0.0


		_MaskCutoff("Cutoff", Range(0,1)) = 0.0
		[HDR] _GlitchColor("GlitchColor", Color) = (1,1,1,1)

		_Perlin("Perlin Noise Mask", 2D) = "white" {}

	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 100

		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Perlin;
			sampler2D _Fractal;
			fixed4 _GlitchColor;
			half _GlitchSpeed;
			half _MaskCutoff;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 uv = float2(_SinTime.g, _SinTime.g);
				int time = _Time * 200 * _GlitchSpeed;
				time %= 20;

				float4 noise = tex2D(_Fractal, i.uv + fixed2(0.12, 0.42) * time);

				return max(0, lerp(float4(0, 0, 0, 0), _GlitchColor * noise, noise - _MaskCutoff) * tex2D(_Perlin, i.uv + _Time.r));

			}
			ENDCG
		}
	}

	SubShader
		{
			Tags{ "RenderType" = "Outline" "Queue" = "Transparent" }
			LOD 100
				Blend One One

			Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _Perlin;
		sampler2D _Fractal;
		fixed4 _GlitchColor;
		half _GlitchSpeed;
		half _MaskCutoff;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		half4 frag(v2f i) : SV_Target
		{
			float2 uv = float2(_SinTime.g, _SinTime.g);
			int time = _Time * 200 * _GlitchSpeed;
			time %= 20;

			half noise = tex2D(_Fractal, i.uv + fixed2(0.12, 0.42) * time).r;

			return fixed4(max(0, lerp(float4(0, 0, 0, 0), noise, noise - _MaskCutoff).r * tex2D(_Perlin, i.uv + _Time.r)));

		}
			ENDCG
		}
	}
}
