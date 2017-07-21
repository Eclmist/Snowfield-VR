Shader "SP/Holographic"
{
	Properties
	{
		_Power("Rim Power", Range(0,10)) = 1
		_Color("Rim Color", Color) = (1,1,1,1)
		_PulseSpeed("PulseSpeed", Range(0,100)) = 1

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005

	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		//Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

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
			half4 _Color;
			half _PulseSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 nDotV = 1 - abs(dot(i.normal, i.viewDir));
				fixed4 col = _Color * pow(nDotV, _Power);
				col.a *= (sin(_PulseSpeed * _Time.x) + 1) * 0.5;

				return col;
			}
			ENDCG
		}

		UsePass "Toon/Basic Outline/OUTLINE"
	}
}
