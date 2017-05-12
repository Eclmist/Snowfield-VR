Shader "Hidden/SSAA_Nearest" 
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

			fixed4 frag(v2f i) : COLOR
			{
				float2 uv = float2(i.texcoord.x * _ResizeWidth, i.texcoord.y * _ResizeHeight);
				float2 f = frac(uv);
				uv = float2(floor(uv.x)/_ResizeWidth, floor(uv.y)/_ResizeHeight);

				return tex2D(_MainTex, uv).rgba;
			}
			ENDCG 

		}
	}
	Fallback Off 
}
