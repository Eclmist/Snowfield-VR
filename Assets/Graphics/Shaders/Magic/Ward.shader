Shader "SP/Magic/Ward"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Normal("Distortion Pattern", 2D) = "white" {}
		_Speed("Speed", Range(-1,1)) = 0
		_DistortionAmt("Distortion Amount", Range(0,1)) = 0
		_Mask("Distortion Mask", 2D) = "white" {}
		_Opacity("Base Opacity", Range(0,1)) = 0

		[HDR]_Color("IntersectionColor", Color) = (1,1,1,1)
		_InvFade("Intersection Factor", Range(0.01,2)) = 1.0

		_OverlayTex("Overlay", 2D) = "white" {}

	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		GrabPass
		{
			"_BackgroundTexture"
		}


		UsePass "SP/Distortion/MAIN"
		UsePass "SP/IntersectionHighlight/MAIN"
		UsePass "SP/AdditiveOverlay/MAIN"

	}
}
