Shader "Custom/CarpetTesselation" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SpecCol("Specular Color", Color) = (1,1,1,1)
		_SpecGlossMap("Specular (RGB), Smoothness (A)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Parallax("Height", Range(0.0, 1.0)) = 0.5
		_FlowPower("Flow Power", Range(0.0, 1.0)) = 0.5
		_ParallaxMap("Heightmap (A)", 2D) = "black" {}
		_ParallaxMapScale("Parallax Map Tiling", Float) = 1
		_ObjSpaceFlowMap("Object Space Flow Map (RGB Linear)", 2D) = "grey" {}
		_EdgeLength("Edge length", Range(0.03,50)) = 10
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
		#pragma surface surf StandardSpecular fullforwardshadows addshadow vertex:disp tessellate:tessEdge

		//#pragma target 3.0

#include "Tessellation.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
		};

		float _EdgeLength;
		float _Parallax;
		float _FlowPower;
		float _ParallaxMapScale;

		float4 tessEdge(appdata v0, appdata v1, appdata v2)
		{
			return UnityEdgeLengthBasedTessCull(v0.vertex, v1.vertex, v2.vertex, _EdgeLength, _Parallax * 1.5f);
		}
		sampler2D _ParallaxMap;
		sampler2D _ObjSpaceFlowMap;
		void disp(inout appdata v)
		{
			float d = tex2Dlod(_ParallaxMap, float4(v.texcoord.xy*_ParallaxMapScale, 0, 0)).a * _Parallax;
			float3 n = tex2Dlod(_ObjSpaceFlowMap, float4(v.texcoord.xy, 0, 0)).rgb;
			n = (n * 2) - 1;
			v.vertex.xyz += ((v.normal + (float3(n.x, 0, n.y)*_FlowPower))*d);
		}

		sampler2D _MainTex;
		sampler2D _SpecGlossMap;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SpecGlossMap;
			float2 uv_BumpMap;

		};

		half _Glossiness;
		fixed4 _Color;
		fixed4 _SpecCol;


		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			fixed4 s = tex2D(_SpecGlossMap, IN.uv_SpecGlossMap);
			o.Specular = s.rgb*_SpecCol.rgb;
			o.Smoothness = s.a*_Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
