
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OutlineRenderer : MonoBehaviour
{
	[SerializeField] private Shader shader;
	[Tooltip ("SP/Outline/Unlitbase i think")]
	[SerializeField] private Shader replacement;
	[SerializeField] private Shader blur;

	[SerializeField] [Range(0,3)] private int downsample = 1;


	[SerializeField] [Range(0,4)] private int blurIteration = 4;
	//[SerializeField] [Range(1, 4)] private int downSample = 2;

	[SerializeField] private Vector2 offset = new Vector2(1,1);

	private Camera cam;
	private Camera tempSecondaryCamera;
	private Material mat;
	private Material blurMat;

	private int camWidth;
	private int camHeight;

	protected void OnEnable()
	{
		cam = GetComponent<Camera>();
		tempSecondaryCamera = new GameObject("Outline Camera").AddComponent<Camera>();
		tempSecondaryCamera.enabled = false;
		Debug.Assert(replacement);
		mat = new Material(shader);
		Debug.Assert(mat);
		blurMat = new Material(blur);
		Debug.Assert(blurMat);

		camWidth = cam.pixelWidth;
		camHeight = cam.pixelHeight;
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//set up a temporary camera
		RenderTexture TempRT = RenderTexture.GetTemporary(camWidth, camHeight, 0);

		blurMat.SetVector ("_BlurSize", offset);

		tempSecondaryCamera.CopyFrom(cam);
		tempSecondaryCamera.clearFlags = CameraClearFlags.Color;
		tempSecondaryCamera.backgroundColor = Color.black;
		tempSecondaryCamera.cullingMask = LayerMask.GetMask("Interactable", "Player");
		tempSecondaryCamera.useOcclusionCulling = false;
		//make the temporary rendertexture

		//set the camera's target texture when rendering
		tempSecondaryCamera.targetTexture = TempRT;

		tempSecondaryCamera.RenderWithShader(replacement, "RenderType");

		RenderTexture blurredTex = RenderTexture.GetTemporary(TempRT.width  / (downsample+1), TempRT.height  / (downsample+1), 0);

		Graphics.Blit (TempRT, blurredTex);

		for (int i = 0; i < blurIteration; i++) 
		{
			RenderTexture temp = RenderTexture.GetTemporary(blurredTex.width, blurredTex.height);

			Graphics.Blit (blurredTex, temp, blurMat, 0); // horizontalPass
			Graphics.Blit (temp, blurredTex, blurMat, 1); // vertical Pass

			RenderTexture.ReleaseTemporary (temp);
		}

		//mat.SetVector("_offset", offset);

		mat.SetTexture("_OutlineBufferTex", TempRT);
		mat.SetTexture("_OutlineBufferTexBlurred", blurredTex);

		Graphics.Blit(source, destination, mat);

		RenderTexture.ReleaseTemporary (blurredTex);
		RenderTexture.ReleaseTemporary (TempRT);
	}
}

