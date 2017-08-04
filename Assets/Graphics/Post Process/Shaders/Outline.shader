Shader "Hidden/ImageEffects/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			
			sampler2D _MainTex;
			sampler2D _OutlineBufferTex;
			sampler2D _OutlineBufferTexBlurred;

			float2 _offset;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 outline = tex2D(_OutlineBufferTex, i.uv);
				fixed4 blur = tex2D(_OutlineBufferTexBlurred, i.uv);

				//fixed4 offset = tex2D(_OutlineBufferTex, i.uv - fixed2(0.01, 0.01) * _offset);
				//offset += tex2D(_OutlineBufferTex, i.uv - fixed2(-0.01, 0.01) * _offset);
				//offset += tex2D(_OutlineBufferTex, i.uv - fixed2(0.01, -0.01) * _offset);
				//offset += tex2D(_OutlineBufferTex, i.uv - fixed2(-0.01, -0.01) * _offset);

				//offset -= outline * 4;
				//offset = max(0, offset);
				blur -= outline;

				blur = max(0, blur);


				fixed4 col = tex2D(_MainTex, i.uv);
				return col + blur;
			}
			ENDCG
		}
	}
}
