
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class OutlineRenderer : MonoBehaviour
{
	[SerializeField] private Shader shader;
	[SerializeField] private Shader replacement;
	//[SerializeField] private Shader blur;


	//[SerializeField] [Range(0,4)] private int blurIteration = 4;
	//[SerializeField] [Range(1, 4)] private int downSample = 2;

	[SerializeField] private Vector2 offset = new Vector2(1,1);

	private Camera camera;
	private Camera tempSecondaryCamera;
	private Material mat;
	//private Material blurMat;
	RenderTexture TempRT;

	protected void OnEnable()
	{
		camera = GetComponent<Camera>();
		tempSecondaryCamera = new GameObject().AddComponent<Camera>();
		tempSecondaryCamera.enabled = false;
		Debug.Assert(replacement);
		mat = new Material(shader);
		Debug.Assert(mat);
		//blurMat = new Material(blur);
		//Debug.Assert(blurMat);

		TempRT = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0);

	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//set up a temporary camera

		tempSecondaryCamera.CopyFrom(camera);
		tempSecondaryCamera.clearFlags = CameraClearFlags.Color;
		tempSecondaryCamera.backgroundColor = Color.black;
		tempSecondaryCamera.cullingMask = LayerMask.GetMask("Interactable");
		tempSecondaryCamera.useOcclusionCulling = false;
		//make the temporary rendertexture

		//set the camera's target texture when rendering
		tempSecondaryCamera.targetTexture = TempRT;

		tempSecondaryCamera.RenderWithShader(replacement, "RenderType");

		//for (int i = 0; i < blurIteration; i++)
		//{ }

		//RenderTexture blurredTex = new RenderTexture(source.width / downSample, source.height / downSample, 0);
		mat.SetVector("_offset", offset);

		//Graphics.Blit(TempRT, blurredTex, blurMat);


		mat.SetTexture("_OutlineBufferTex", TempRT);
		//mat.SetTexture("_OutlineBufferTexBlurred", blurredTex);

		Graphics.Blit(source, destination, mat);

		//blurredTex.Release();
	}


	protected void OnDisable()
	{
		TempRT.Release();
	}
}

