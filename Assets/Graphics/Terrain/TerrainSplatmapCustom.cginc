// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef TERRAIN_SPLATMAP_CUSTOM_CGINC_INCLUDED
#define TERRAIN_SPLATMAP_CUSTOM_CGINC_INCLUDED
struct Input
{
	float2 uv_Splat0 : TEXCOORD0;
	float2 uv_Splat1 : TEXCOORD1;
	float2 uv_Splat2 : TEXCOORD2;
	float2 uv_Splat3 : TEXCOORD3;
	float2 tc_Control : TEXCOORD4;	// Not prefixing '_Contorl' with 'uv' allows a tighter packing of interpolators, which is necessary to support directional lightmap.
	UNITY_FOG_COORDS(5)
			float3 worldPos;
			#ifdef _TERRAIN_NORMAL_MAP
		float3 vertNormal;
	#endif
};
sampler2D _Control;
float4 _Control_ST;
float _tiles0x;
float _tiles0y;
float _tiles0z;
float _tiles1x;
float _tiles1y;
float _tiles1z;
float _tiles2x;
float _tiles2y;
float _tiles2z;
float _tiles3x;
float _tiles3y;
float _tiles3z;

float _offset0x;
float _offset0y;
float _offset0z;
float _offset1x;
float _offset1y;
float _offset1z;
float _offset2x;
float _offset2y;
float _offset2z;
float _offset3x;
float _offset3y;
float _offset3z;

float4 _Color0;
float4 _Color1;
float4 _Color2;
float4 _Color3;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
fixed3 normal;
float3 worldPos;
#ifdef _TERRAIN_NORMAL_MAP
	sampler2D _Normal0, _Normal1, _Normal2, _Normal3;
#endif
void SplatmapVert(inout appdata_full v, out Input data)
{
	UNITY_INITIALIZE_OUTPUT(Input, data);
	data.tc_Control = TRANSFORM_TEX(v.texcoord, _Control);	// Need to manually transform uv here, as we choose not to use 'uv' prefix for this texcoord.
	float4 pos = UnityObjectToClipPos (v.vertex); 
	UNITY_TRANSFER_FOG(data, pos);
#ifdef _TERRAIN_NORMAL_MAP
	v.tangent.xyz = cross(v.normal, float3(0,0,1));
	v.tangent.w = -1;
	data.vertNormal = v.normal;
#endif
}

#ifdef TERRAIN_STANDARD_SHADER
void SplatmapMix(Input IN, half4 defaultAlpha, out half4 splat_control, out half weight, out fixed4 mixedDiffuse, inout fixed3 mixedNormal)
#else
void SplatmapMix(Input IN, out half4 splat_control, out half weight, out fixed4 mixedDiffuse, inout fixed3 mixedNormal)
#endif
{
	splat_control = tex2D(_Control, IN.tc_Control);
	weight = dot(splat_control, half4(1,1,1,1));
	#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
		clip(weight - 0.0039 /*1/255*/);
	#endif
	worldPos = IN.worldPos;
	#ifdef _TERRAIN_NORMAL_MAP
		normal = abs(IN.vertNormal);
	#else
		normal = abs(mixedNormal);
	#endif
	// Normalize weights before lighting and restore weights in final modifier functions so that the overal
	// lighting result can be correctly weighted.
	splat_control /= (weight + 1e-3f);
	mixedDiffuse = 0.0f;
	#ifdef TERRAIN_STANDARD_SHADER
		float y = normal.y;
		float z = normal.z;
		float2 y0 = IN.worldPos.zy * _tiles0x + _offset0x;
		float2 x0 = IN.worldPos.xz * _tiles0z + _offset0z;
		float2 z0 = IN.worldPos.xy * _tiles0y + _offset0y;
		fixed4 cX0 = tex2D(_Splat0, y0);
		fixed4 cY0 = tex2D(_Splat0, x0);
		fixed4 cZ0 = tex2D(_Splat0, z0);
		float4 side0 = lerp(cX0, cZ0, z);
		float4 top0 = lerp(side0, cY0, y);
		mixedDiffuse += splat_control.r * top0 * _Color0; 
		
		float2 y1 = IN.worldPos.zy * _tiles1x + _offset1x;
		float2 x1 = IN.worldPos.xz * _tiles1z + _offset1z;
		float2 z1 = IN.worldPos.xy * _tiles1y + _offset1y;
		fixed4 cX1 = tex2D(_Splat1, y1);
		fixed4 cY1 = tex2D(_Splat1, x1);
		fixed4 cZ1 = tex2D(_Splat1, z1);
		float4 side1 = lerp(cX1, cZ1, z);
		float4 top1 = lerp(side1, cY1, y);
		mixedDiffuse += splat_control.g * top1 * _Color1; 
	
		float2 y2 = IN.worldPos.zy * _tiles2x + _offset2x;
		float2 x2 = IN.worldPos.xz * _tiles2z + _offset2z;
		float2 z2 = IN.worldPos.xy * _tiles2y + _offset2y;
		fixed4 cX2 = tex2D(_Splat2, y2);
		fixed4 cY2 = tex2D(_Splat2, x2);
		fixed4 cZ2 = tex2D(_Splat2, z2);
		float4 side2 = lerp(cX2, cZ2, z);
		float4 top2 = lerp(side2, cY2, y);
		mixedDiffuse += splat_control.b * top2 * _Color2; 
		
		float2 y3 = IN.worldPos.zy * _tiles3x + _offset3x;
		float2 x3 = IN.worldPos.xz * _tiles3z + _offset3z;
		float2 z3 = IN.worldPos.xy * _tiles3y + _offset3y;
		fixed4 cX3 = tex2D(_Splat3, y3);
		fixed4 cY3 = tex2D(_Splat3, x3);
		fixed4 cZ3 = tex2D(_Splat3, z3);
		float4 side3 = lerp(cX2, cZ2, z);
		float4 top3 = lerp(side3, cY3, y);
		mixedDiffuse += splat_control.a * top3 * _Color3; 
		
	//	mixedDiffuse = splat_control.r * y;  
	//	mixedDiffuse += splat_control.r * tex2D(_Splat0, IN.uv_Splat0) * half4(1.0, 1.0, 1.0, defaultAlpha.r);
	//	mixedDiffuse += splat_control.g * tex2D(_Splat1, IN.uv_Splat1) * half4(1.0, 1.0, 1.0, defaultAlpha.g);
	//	mixedDiffuse += splat_control.b * tex2D(_Splat2, IN.uv_Splat2) * half4(1.0, 1.0, 1.0, defaultAlpha.b);
	//	mixedDiffuse += splat_control.a * tex2D(_Splat3, IN.uv_Splat3) * half4(1.0, 1.0, 1.0, defaultAlpha.a);
	#else
		float y = normal.y;
		float z = normal.z;
		float2 y0 = IN.worldPos.zy * _tiles0x + _offset0x;
		float2 x0 = IN.worldPos.xz * _tiles0z + _offset0z;
		float2 z0 = IN.worldPos.xy * _tiles0y + _offset0y;
		fixed4 cX0 = tex2D(_Splat0, y0);
		fixed4 cY0 = tex2D(_Splat0, x0);
		fixed4 cZ0 = tex2D(_Splat0, z0);
		float4 side0 = lerp(cX0, cZ0, z);
		float4 top0 = lerp(side0, cY0, y);
		mixedDiffuse += splat_control.r * top0 * _Color0; 
		
		float2 y1 = IN.worldPos.zy * _tiles1x + _offset1x;
		float2 x1 = IN.worldPos.xz * _tiles1z + _offset1z;
		float2 z1 = IN.worldPos.xy * _tiles1y + _offset1y;
		fixed4 cX1 = tex2D(_Splat1, y1);
		fixed4 cY1 = tex2D(_Splat1, x1);
		fixed4 cZ1 = tex2D(_Splat1, z1);
		float4 side1 = lerp(cX1, cZ1, z);
		float4 top1 = lerp(side1, cY1, y);
		mixedDiffuse += splat_control.g * top1 * _Color1; 
	
		float2 y2 = IN.worldPos.zy * _tiles2x + _offset2x;
		float2 x2 = IN.worldPos.xz * _tiles2z + _offset2z;
		float2 z2 = IN.worldPos.xy * _tiles2y + _offset2y;
		fixed4 cX2 = tex2D(_Splat2, y2);
		fixed4 cY2 = tex2D(_Splat2, x2);
		fixed4 cZ2 = tex2D(_Splat2, z2);
		float4 side2 = lerp(cX2, cZ2, z);
		float4 top2 = lerp(side2, cY2, y);
		mixedDiffuse += splat_control.b * top2 * _Color2; 
		
		float2 y3 = IN.worldPos.zy * _tiles3x + _offset3x;
		float2 x3 = IN.worldPos.xz * _tiles3z + _offset3z;
		float2 z3 = IN.worldPos.xy * _tiles3y + _offset3y;
		fixed4 cX3 = tex2D(_Splat3, y3);
		fixed4 cY3 = tex2D(_Splat3, x3);
		fixed4 cZ3 = tex2D(_Splat3, z3);
		float4 side3 = lerp(cX2, cZ2, z);
		float4 top3 = lerp(side3, cY3, y);
		mixedDiffuse += splat_control.a * top3 * _Color3; 
		
	//	mixedDiffuse += splat_control.r * tex2D(_Splat0, IN.uv_Splat0);
	//	mixedDiffuse += splat_control.g * tex2D(_Splat1, IN.uv_Splat1);
	//	mixedDiffuse += splat_control.b * tex2D(_Splat2, IN.uv_Splat2);
	//	mixedDiffuse += splat_control.a * tex2D(_Splat3, IN.uv_Splat3);
	#endif
	#ifdef _TERRAIN_NORMAL_MAP
		fixed4 nrm = 0.0f;
		fixed4 nX0 = tex2D(_Normal0, y0);
		fixed4 nY0 = tex2D(_Normal0, x0);
		fixed4 nZ0 = tex2D(_Normal0, z0);
		float4 side0n = lerp(nX0, nZ0, z);
		float4 top0n = lerp(side0n, nY0, y);
		nrm += splat_control.r * top0n;
		
		fixed4 nX1 = tex2D(_Normal1, y1);
		fixed4 nY1 = tex2D(_Normal1, x1);
		fixed4 nZ1 = tex2D(_Normal1, z1);
		float4 side1n = lerp(nX1, nZ1, z);
		float4 top1n = lerp(side1n, nY1, y);
		nrm += splat_control.g * top1n;
		
		fixed4 nX2 = tex2D(_Normal2, y2);
		fixed4 nY2 = tex2D(_Normal2, x2);
		fixed4 nZ2 = tex2D(_Normal2, z2);
		float4 side2n = lerp(nX2, nZ2, z);
		float4 top2n = lerp(side2n, nY2, y);
		nrm += splat_control.b * top2n;

		fixed4 nX3 = tex2D(_Normal3, y3);
		fixed4 nY3 = tex2D(_Normal3, x3);
		fixed4 nZ3 = tex2D(_Normal3, z3);
		float4 side3n = lerp(nX3, nZ3, z);
		float4 top3n = lerp(side3n, nY3, y);
		nrm += splat_control.a * top3n;

		
	//	nrm += splat_control.r * tex2D(_Normal0, IN.uv_Splat0);
	//	nrm += splat_control.g * tex2D(_Normal1, IN.uv_Splat1);
	//	nrm += splat_control.b * tex2D(_Normal2, IN.uv_Splat2);
	//	nrm += splat_control.a * tex2D(_Normal3, IN.uv_Splat3);
		mixedNormal = UnpackNormal(nrm);
	#endif
}

#ifndef TERRAIN_SURFACE_OUTPUT
	#define TERRAIN_SURFACE_OUTPUT SurfaceOutput
#endif

void SplatmapFinalColor(Input IN, TERRAIN_SURFACE_OUTPUT o, inout fixed4 color)
{
	color *= o.Alpha;
	#ifdef TERRAIN_SPLAT_ADDPASS
		UNITY_APPLY_FOG_COLOR(IN.fogCoord, color, fixed4(0,0,0,0));
	#else
		UNITY_APPLY_FOG(IN.fogCoord, color);
	#endif
}

void SplatmapFinalPrepass(Input IN, TERRAIN_SURFACE_OUTPUT o, inout fixed4 normalSpec)
{
	normalSpec *= o.Alpha;
}

void SplatmapFinalGBuffer(Input IN, TERRAIN_SURFACE_OUTPUT o, inout half4 diffuse, inout half4 specSmoothness, inout half4 normal, inout half4 emission)
{
	diffuse.rgb *= o.Alpha;
	specSmoothness *= o.Alpha;
	normal.rgb *= o.Alpha;
	emission *= o.Alpha;
}

#endif // TERRAIN_SPLATMAP_COMMON_CGINC_INCLUDED