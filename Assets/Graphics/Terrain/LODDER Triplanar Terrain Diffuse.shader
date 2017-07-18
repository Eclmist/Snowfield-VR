Shader "LODDER/Ttriplanar Terrain Diffuse" {
	Properties {
		[HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
		[HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
		[HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
		// used in fallback on old cards & base map
		[HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
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

	CGINCLUDE
		#pragma surface surf Lambert vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer
		#pragma multi_compile_fog
		#pragma target 3.0
		#pragma debug
		#include "TerrainSplatmapCustom.cginc"


		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 splat_control;
			half weight;
			fixed4 mixedDiffuse;
			SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
		}
	ENDCG

	Category {
		Tags {
			"SplatCount" = "4"
			"Queue" = "Geometry-99"
			"RenderType" = "Opaque"
		}
		// TODO: Seems like "#pragma target 3.0 _TERRAIN_NORMAL_MAP" can't fallback correctly on less capable devices?
		// Use two sub-shaders to simulate different features for different targets and still fallback correctly.
		SubShader { // for sm3.0+ targets
			CGPROGRAM
				#pragma target 3.0
				#pragma multi_compile __ _TERRAIN_NORMAL_MAP
			ENDCG
		}
		SubShader { // for sm2.0 targets
			CGPROGRAM
			ENDCG
		}
	}

	Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Diffuse-AddPass"
	Dependency "BaseMapShader" = "Diffuse"
	Dependency "Details0"      = "Hidden/TerrainEngine/Details/Vertexlit"
	Dependency "Details1"      = "Hidden/TerrainEngine/Details/WavingDoublePass"
	Dependency "Details2"      = "Hidden/TerrainEngine/Details/BillboardWavingDoublePass"
	Dependency "Tree0"         = "Hidden/TerrainEngine/BillboardTree"

	Fallback "Diffuse"
}
