using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GenericPostProcess : MonoBehaviour
{
	[SerializeField] protected Shader shader;

	protected Camera camera;
	protected Material mat;

	protected virtual void Start()
	{
		camera = GetComponent<Camera>();
		mat = new Material(shader);

		Debug.Assert(mat);
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
	}

}
