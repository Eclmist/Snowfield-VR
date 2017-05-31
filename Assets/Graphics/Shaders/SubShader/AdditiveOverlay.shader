Shader "SP/AdditiveOverlay"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OverlayTex("Overlay", 2D) = "white" {}
		[HDR]_Color("Tint", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend One One
			Cull Off
			ZWrite Off
		Pass
		{
			Name "Main"

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
			sampler2D _OverlayTex;
			sampler2D _Normal;
			float4 _MainTex_ST;
			float4 _Color;
			float _Opacity;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color / 10 * _Opacity * tex2D(_OverlayTex, i.uv);
				col.a -= 0.1;
				return col;
			}
			ENDCG
		}
	}
}
