using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCamera : MonoBehaviour {

	[SerializeField] Material glitchMat;
	[SerializeField] private bool debug;

	[SerializeField] private Shader replacement;
	[SerializeField] private Shader composit;

	private Camera tempSecondaryCamera;
	private Material mat;
	private int camWidth;
	private int camHeight;
	private Camera cam;

	private void Start()
	{
		cam = GetComponent<Camera>();
		tempSecondaryCamera = new GameObject("Glitch Camera").AddComponent<Camera>();
		tempSecondaryCamera.enabled = false;
		Debug.Assert(replacement);
		mat = new Material(composit);
		Debug.Assert(mat);

		camWidth = cam.pixelWidth;
		camHeight = cam.pixelHeight;
		SetGlobalShaders();
	}
	protected void OnDisable()
	{
		Destroy(tempSecondaryCamera.gameObject);
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (debug)
		{       // TODO: move to onenable
			SetGlobalShaders();
		}

		//set up a temporary camera
		RenderTexture TempRT = RenderTexture.GetTemporary(camWidth, camHeight, 0);

		tempSecondaryCamera.CopyFrom(cam);
		tempSecondaryCamera.clearFlags = CameraClearFlags.SolidColor;
		tempSecondaryCamera.backgroundColor = Color.black;

		tempSecondaryCamera.useOcclusionCulling = false;
		//make the temporary rendertexture

		//set the camera's target texture when rendering
		tempSecondaryCamera.targetTexture = TempRT;

		tempSecondaryCamera.RenderWithShader(replacement, "RenderType");

		mat.SetTexture("_GlitchTex", TempRT);

		Graphics.Blit(source, destination, mat);

		RenderTexture.ReleaseTemporary(TempRT);
	}

	void SetGlobalShaders()
	{
		Shader.SetGlobalColor("_GlitchColor", glitchMat.GetColor("_GlitchColor"));
		Shader.SetGlobalFloat("_GlitchSpeed", glitchMat.GetFloat("_GlitchSpeed"));
		Shader.SetGlobalFloat("_MaskCutoff", glitchMat.GetFloat("_MaskCutoff"));
		Shader.SetGlobalTexture("_Fractal", glitchMat.GetTexture("_Fractal"));
		Shader.SetGlobalTexture("_Perlin", glitchMat.GetTexture("_Perlin"));
	}
}
