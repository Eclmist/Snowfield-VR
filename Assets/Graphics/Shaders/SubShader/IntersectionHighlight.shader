Shader "SP/IntersectionHighlight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HDR]_Color("IntersectionColor", Color) = (1,1,1,1)
		_InvFade("Intersection Factor", Range(0.01,2)) = 1.0

	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off

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
				float4 projPos : TEXCOORD1;

			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 _MainTex_ST;
			float4 _Color;
			float _Opacity;
			float _InvFade;
			sampler2D _Mask;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.projPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(0,0,0,0);
				//Intersection Highlight
				float sceneZ = LinearEyeDepth(
					UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))
				);
				float fragZ = i.projPos.z;
				float fade = saturate(_InvFade* (sceneZ - fragZ));

				//return fixed4(0, 0, 0, 0);

				col.rgb += _Color.rgb;
				col.a = (1 - fade) * tex2D(_Mask, i.uv) * _Opacity;
				return col;
			}
			ENDCG
		}
	}
}
