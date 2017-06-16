using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class GenericPostProcess : MonoBehaviour
{
	[SerializeField] private Shader shader;

	private Camera camera;
	private Material mat;

	protected void Awake()
	{
		camera = GetComponent<Camera>();
		mat = new Material(shader);

		Debug.Assert(mat);
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
	}

}
