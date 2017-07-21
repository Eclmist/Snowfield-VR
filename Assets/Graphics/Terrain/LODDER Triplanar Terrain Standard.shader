Shader "LODDER/Triplanar Terrain Standard" {
	Properties {
		// set by terrain engine
		[HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
		[HideInInspector] [Gamma] _Metallic0 ("Metallic 0", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic1 ("Metallic 1", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic2 ("Metallic 2", Range(0.0, 1.0)) = 0.0	
		[HideInInspector] [Gamma] _Metallic3 ("Metallic 3", Range(0.0, 1.0)) = 0.0
		[HideInInspector] _Smoothness0 ("Smoothness 0", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness1 ("Smoothness 1", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness2 ("Smoothness 2", Range(0.0, 1.0)) = 1.0	
		[HideInInspector] _Smoothness3 ("Smoothness 3", Range(0.0, 1.0)) = 1.0

		// used in fallback on old cards & base map
		[HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
	//	_Normals0 ("Normals0", Range(0.001, 5.0)) = 1
	//	_Normals1 ("Normals1", Range(0.001, 5.0)) = 1
	//	_Normals2 ("Normals2", Range(0.001, 5.0)) = 1
	//	_Normals3 ("Normals3", Range(0.001, 5.0)) = 1
		 _Color0 ("Color0", Color) = (1,1,1,1)
		 _Color1 ("Color1", Color) = (1,1,1,1)
		 _Color2 ("Color2", Color) = (1,1,1,1)
		 _Color3 ("Color3", Color) = (1,1,1,1)
		 _tiles0x ("tile0X", float) = 0.03
		 _tiles0y ("tile0Y", float) = 0.03
		 _tiles0z ("tile0Z", float) = 0.03
		 _tiles1x ("tile1X", float) = 0.03
		 _tiles1y ("tile1Y", float) = 0.03
		 _tiles1z ("tile1Z", float) = 0.03
		 _tiles2x ("tile2X", float) = 0.03
		 _tiles2y ("tile2Y", float) = 0.03
		 _tiles2z ("tile2Z", float) = 0.03
		 _tiles3x ("tile3X", float) = 0.03
		 _tiles3y ("tile3Y", float) = 0.03
		 _tiles3z ("tile3Z", float) = 0.03
		 _offset0x ("offset0X", float) = 0
		 _offset0y ("offset0Y", float) = 0
		 _offset0z ("offset0Z", float) = 0
		 _offset1x ("offset1X", float) = 0
		 _offset1y ("offset1Y", float) = 0
		 _offset1z ("offset1Z", float) = 0
		 _offset2x ("offset2X", float) = 0
		 _offset2y ("offset2Y", float) = 0
		 _offset2z ("offset2Z", float) = 0
		 _offset3x ("offset3X", float) = 0
		 _offset3y ("offset3Y", float) = 0
		 _offset3z ("offset3Z", float) = 0
	}

	SubShader {
		Tags {
			"SplatCount" = "4"
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer fullforwardshadows
		#pragma multi_compile_fog
		#pragma target 3.0
		// needs more than 8 texcoords
		#pragma exclude_renderers gles
		#include "UnityPBSLighting.cginc"

		#pragma multi_compile __ _TERRAIN_NORMAL_MAP

		#define TERRAIN_STANDARD_SHADER
		#define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
		#include "TerrainSplatmapCustom.cginc"

		half _Metallic0;
		half _Metallic1;
		half _Metallic2;
		half _Metallic3;
		
		half _Smoothness0;
		half _Smoothness1;
		half _Smoothness2;
		half _Smoothness3;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 splat_control;
			half weight;
			fixed4 mixedDiffuse;
			half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
			SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
			o.Smoothness = mixedDiffuse.a;
			o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
		}
		ENDCG
	}

	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
	Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

	Fallback "Nature/Terrain/Diffuse"
}
