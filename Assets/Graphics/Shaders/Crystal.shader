Shader "SP/Portal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 screenPos : TEXCOORD1;
				half3 worldNormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _SecondaryCameraTex;
			float4 _MainTex_ST;
			float4 _Color;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = float3((o.vertex.xy + o.vertex.w) * 0.5, o.vertex.w);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 correctedScreenUV = i.screenPos.xy / i.screenPos.z;
				correctedScreenUV.y = 1 - correctedScreenUV.y;

				float3 viewSpaceNormals = mul(UNITY_MATRIX_V, i.worldNormal);
				fixed4 normal = tex2D(_MainTex, i.uv);
				viewSpaceNormals += normal.rgb;
				fixed4 hiddenTex = tex2D(_SecondaryCameraTex, correctedScreenUV + viewSpaceNormals *0.01);
				//return fixed4(correctedScreenUV, 0, 1);
				return hiddenTex * _Color;
			}
			ENDCG
		}
	}
}
