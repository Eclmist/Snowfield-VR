using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GlitchCamera : MonoBehaviour {

	[SerializeField] Material glitchMat;
	[SerializeField] [Range(0,1)] private float glitchAmount = 0;
	[SerializeField] [Range(-1, 4)] private float worldYOutline = -1;

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
		RenderTexture TempRT = RenderTexture.GetTemporary(camWidth/ 2, camHeight/2, 16, RenderTextureFormat.RHalf);
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

		RaycastCornerBlit(source, destination, mat);

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
		glitchAmount = amount;
		Shader.SetGlobalFloat("_MaskCutoff", 1 - glitchAmount * correctedMinMask);
		mat.SetFloat("_worldY", amount * 30 - 1);


		if (glitch2)
		{
			if (amount > 0.1F)
			{
				glitch2.enabled = true;
				glitch2._intensity = (amount / 2);

			}
			else
			{
				glitch2.enabled = false;
			}
		}
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = cam.farClipPlane;
		float camFov = cam.fieldOfView;
		float camAspect = cam.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (cam.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (cam.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (cam.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (cam.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}

}
