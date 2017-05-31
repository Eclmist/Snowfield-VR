Shader "SP/Distortion"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Normal("Distortion Pattern", 2D) = "white" {}
		_Mask("Distortion Mask", 2D) = "white" {}
		_Speed("Speed", Range(-1,1)) = 0
		_DistortionAmt("Distortion Amount", Range(0,1)) = 0
		_Opacity("Base Opacity", Range(0,1)) = 0
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off

		GrabPass
		{
			"_BackgroundTexture"
		}

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
			fixed4 color : COLOR;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 grabPos : TEXCOORD2;
			fixed4 color : COLOR;
		};

		sampler2D _MainTex;
		sampler2D _CameraDepthTexture;
		sampler2D _Normal;
		sampler2D _BackgroundTexture;
		sampler2D _Mask;
		float4 _MainTex_ST;
		float _DistortionAmt;
		float _Speed;
		float _Opacity;

		fixed4 _TintColor;
		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.grabPos = ComputeGrabScreenPos(o.vertex);
			o.color = v.color;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{	
			//Distortion
			float4 distortion = tex2D(_Normal, i.uv +
			(_Time.rg * _Speed));


			distortion -= float4(0.5, 0.5, 0.5, 0.5);

			float4 distortedTex = tex2Dproj(_BackgroundTexture, i.grabPos + distortion * _DistortionAmt);
			distortedTex.a = tex2D(_Mask, i.uv) * _Opacity * i.color.a;
			return distortedTex;

		}
			ENDCG
		}
	}
}