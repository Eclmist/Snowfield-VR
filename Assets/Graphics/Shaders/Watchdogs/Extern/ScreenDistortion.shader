Shader "Hidden/ScreenDistortion"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_dispTex("Base (RGB)", 2D) = "bump" {}
		_intensity("Glitch Intensity", float) = 0.5
		_aberration("Aberration Strength", float) = 0
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _dispTex;
			float _intensity;
			float _aberration;
			float flip_up;
			float flip_down;
			float displace;
			float scale;


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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
				half4 _MainTex_ST;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 ooo = (i.uv - 0.5) * 2.0;
				float2 uv;
				uv.x = (1 - ooo.y * ooo.y) * sqrt(_intensity) *ooo.x;
				uv.y = (1 - ooo.x * ooo.x) * sqrt(_intensity) *ooo.y;


				float4 normal = tex2D(_dispTex, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST));
				//return normal;

				if (i.uv.y < flip_up)
					i.uv.y = 1 - (uv.y + flip_up);

				if (i.uv.y > flip_down)
					i.uv.y = 1 - (uv.y - flip_down);

				i.uv.xy += (normal.xy - 0.5) *displace / 2;



				float2 rectangle = float2(i.uv.x - 0.5, i.uv.y - 0.5);
				float dist = sqrt(pow(rectangle.x, 2) + pow(rectangle.y, 2));

				float mov = _aberration * dist / 2;
				float2 uvR, uvG, uvB = (float2)0;

				uvR = float2(i.uv.x - mov, i.uv.y - mov);
				uvG = float2(i.uv.x + mov, i.uv.y + mov);
				uvB = float2(i.uv.x - abs(mov / 2), i.uv.y + abs(mov / 2));

				float sumR = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uvR, _MainTex_ST)).r;
				float sumG = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uvG, _MainTex_ST)).g;
				float sumB = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uvB, _MainTex_ST)).b;
				return fixed4(sumR, sumG, sumB, 1);
			}
			ENDCG
		}
	}
}
