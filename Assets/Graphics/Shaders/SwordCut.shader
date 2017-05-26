Shader "SP/SwordCut"
{
	Properties
	{
		_StencilMask("Stencil Mask", Int) = 60

		_MainTex("Texture", 2D) = "white" {}
		_InnerTexture("Inner Texture", 2D) = "white" {}

		[HDR] _TintColor("Color", Color) = (1, 0, 0, 0)
		[HDR] _InnerColor("Color", Color) = (1, 1, 0, 0)

		_SceneZOffset("Scene Z Offset", Range(0,1)) = 0.00005
		_DepthOffset("ZTest Offset", Range(0,0.5)) = 0.00005
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 100

		Blend One One
		Cull Off
		
		ZTest Always
		ZWrite Off

		Stencil
		{
			Ref[_StencilMask]
			Comp equal
			Pass Keep
			Fail Keep
		}

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
				float depth : DEPTH;
				float4 projPos : TEXCOORD1;
				float3 fragPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			sampler2D _InnerTexture;

			float4 _MainTex_ST;
			fixed4 _TintColor;
			fixed4 _InnerColor;

			float _DepthOffset;
			float _SceneZOffset;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.projPos = ComputeScreenPos(o.vertex);
				o.depth = COMPUTE_EYEDEPTH(v.vertex);
				
				o.fragPos = float3(o.vertex.xy, o.vertex.w);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// sample the texturedebug shaders
				fixed4 col = tex2D(_MainTex, i.uv);

				float2 correctedUV = (i.fragPos.xy / i.fragPos.z + 1) * 0.5;
				fixed4 innerTex = (1 - tex2D(_InnerTexture, correctedUV * 5)) * _InnerColor * col;
				float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))) +_SceneZOffset;
				
				if (abs(i.depth - sceneZ) > _DepthOffset) discard;
				
				return col * _TintColor + innerTex;
			}
			ENDCG
		}
	}
}
