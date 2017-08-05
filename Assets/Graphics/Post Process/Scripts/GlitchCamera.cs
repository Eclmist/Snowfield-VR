using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GlitchCamera : MonoBehaviour {

	[SerializeField] Material glitchMat;
	[SerializeField] [Range(0,1)] private float glitchAmount = 1;

	[SerializeField] [Range(0, 1)] private float minMask = 0.7F;
	private float correctedMinMask;

	[SerializeField] private Shader replacement;
	[SerializeField] private Shader composit;

	[SerializeField]
	private Glitch2 glitch2;


	private Camera tempSecondaryCamera;
	private Material mat;
	private int camWidth;
	private int camHeight;
	private Camera cam;

	private void OnEnable()
	{
		correctedMinMask = 1 - minMask;
		if (glitch2)
		{
			glitch2.enabled = true;
		}

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
		glitch2.enabled = false;
		if (tempSecondaryCamera)
		DestroyImmediate(tempSecondaryCamera.gameObject);
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Application.isEditor)
		{      
			SetGlobalShaders();
		}

		//set up a temporary camera
		RenderTexture TempRT = RenderTexture.GetTemporary(camWidth, camHeight, 16, RenderTextureFormat.RHalf);
		tempSecondaryCamera.CopyFrom(cam);
		tempSecondaryCamera.clearFlags = CameraClearFlags.SolidColor;
		tempSecondaryCamera.backgroundColor = Color.black;

		tempSecondaryCamera.useOcclusionCulling = false;
		//make the temporary rendertexture

		//set the camera's target texture when rendering
		tempSecondaryCamera.targetTexture = TempRT;

		tempSecondaryCamera.depthTextureMode = DepthTextureMode.Depth;
		tempSecondaryCamera.RenderWithShader(replacement, "RenderType");

		mat.SetTexture("_GlitchTex", TempRT);

		Graphics.Blit(source, destination, mat);

		RenderTexture.ReleaseTemporary(TempRT);
	}

	void SetGlobalShaders()
	{
		Shader.SetGlobalColor("_GlitchColor", glitchMat.GetColor("_GlitchColor"));
		Shader.SetGlobalFloat("_GlitchSpeed", glitchMat.GetFloat("_GlitchSpeed"));
		Shader.SetGlobalTexture("_Fractal", glitchMat.GetTexture("_Fractal"));
		Shader.SetGlobalTexture("_Perlin", glitchMat.GetTexture("_Perlin"));

		SetGlitchAmount(glitchAmount);
	}

	public void SetGlitchAmount(float amount)
	{
		Shader.SetGlobalFloat("_MaskCutoff", 1 - glitchAmount * correctedMinMask);
		glitch2._intensity = amount;
	}
}
