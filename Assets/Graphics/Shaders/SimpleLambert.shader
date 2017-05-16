Shader "Custom/SimpleLambert" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		//_RampTex("Ramp", 2D) = "white" {}
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex("Ramp", 2D) = "white" {}	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Toon

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RampTex;
		
		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir,
			fixed atten)
		{
			half NdotL = dot(s.Normal, lightDir);
			NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5));
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
			c.a = s.Alpha;
			return c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
