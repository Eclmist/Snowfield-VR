Shader "Hidden/SSAA_Bilinear" 
{
	Properties 
	{
	 _MainTex ("Texture", 2D) = "" {} 
	}
	SubShader {
		
		Pass {
 			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "SSAA_Utils.cginc"

			sampler2D _MainTex;
			float _ResizeHeight;
			float _ResizeWidth;
			float _Sharpness;
			float _SampleDistance;

			fixed4 frag(v2f i) : COLOR
			{
				float squareW = (_SampleDistance / _ResizeWidth);
				float squareH = (_SampleDistance / _ResizeHeight);
				// neighbor pixels
				float4 top = tex2D(_MainTex, i.texcoord + float2(0.0f, -squareH));
				float4 left = tex2D(_MainTex, i.texcoord + float2(-squareW, 0.0f));
				float4 mid = tex2D(_MainTex, i.texcoord  + float2(0.0f, 0.0f));
				float4 right = tex2D(_MainTex, i.texcoord + float2(squareW, 0.0f));
				float4 bot = tex2D(_MainTex, i.texcoord + float2(0.0f, squareH));
				// avg
				float4 sampleaverage = (top + left + right + bot) / 4;

				// lerp based on sharpness
				return lerp(sampleaverage, mid, _Sharpness);
			}
			ENDCG 

		}
	}
	Fallback Off 
}
