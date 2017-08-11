Shader "Hidden/GlitchComposit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;

			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_depth = v.uv.xy;
				o.interpolatedRay = v.ray;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _GlitchTex;
			sampler2D _CameraDepthTexture;
			float4 _GlitchColor;
			half _worldY;
			half4 _MainTex_ST;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 fixedUV = UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST);

				fixed4 col = tex2D(_MainTex, fixedUV);
				fixed4 glitch = tex2D(_GlitchTex, fixedUV);

				fixed colLeft = tex2D(_MainTex, i.uv + fixed2(0.0008, 0)).r;
				fixed colRight = tex2D(_MainTex, i.uv + fixed2(-0.0008, 0)).r;
				fixed colUp = tex2D(_MainTex, i.uv + fixed2(0, 0.0014)).r;
				fixed colDown = tex2D(_MainTex, i.uv + fixed2(0, -0.0014)).r;

				//if (abs(depthDown - depth) > 0.00001 && abs(colDown - col.r) > 0.001) col += fixed4(0, 0.4, 1, 1);
				//if (abs(depthUp - depth) > 0.00001 && abs(colUp - col.r) > 0.001) col += fixed4(0, 0.4, 1, 1);
				//if (abs(depthRight - depth) > 0.00001 && abs(colRight - col.r) > 0.001) col += fixed4(0, 0.4, 1, 1);
				//if (abs(depthLeft - depth) > 0.00001 && abs(colLeft - col.r) > 0.001) col += fixed4(0, 0.4, 1, 1);

				//return depth + depthLeft + depthUp;

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv_depth, _MainTex_ST)));
				float linearDepth = Linear01Depth(rawDepth);


				fixed outline = 0;

				float3 fragPos = _WorldSpaceCameraPos + i.interpolatedRay *
					linearDepth;

				//return half4(fragPosY, 1,1,1 );
				if (fragPos.y < _worldY && _worldY > 0 && fragPos.y > _worldY - 0.3)
				{
					outline += pow(abs(colDown - col.r), 2.2);
					outline += pow(abs(colUp - col.r), 2.2);
					outline += pow(abs(colRight - col.r), 2.2);
					outline += pow(abs(colLeft - col.r), 2.2);

					outline *= fragPos.y - (_worldY - 0.3);
				}

				return col + glitch.r * _GlitchColor +min(0.8, (outline) * 100000000) * fixed4(0, 1, 1, 1);
			}
			ENDCG
		}
	}
}
