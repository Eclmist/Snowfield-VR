Shader "SP/Rim"
{
	Properties
	{
		_Power("Rim Power", Range(0,10)) = 1
		_Opacity("Rim Opacity", Range(0,1)) = 1
		_Color("Rim Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		Cull Off
		Blend One One

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normal : TEXCOORD2;
			};

			half _Power;
			half _Opacity;
			half4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.normal = UnityObjectToWorldNormal(v.normal);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 nDotV = 1 - abs(dot(i.normal, i.viewDir));

				return _Color * _Opacity * pow(nDotV, _Power);

			}
			ENDCG
		}
	}
}
