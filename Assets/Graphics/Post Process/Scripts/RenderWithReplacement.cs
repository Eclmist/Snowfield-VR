using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class RenderWithReplacement : MonoBehaviour {

	[SerializeField] protected Shader shader;
	[SerializeField] protected string replacementTag;
	protected Camera cam;

	protected virtual void Awake()
	{
		cam = GetComponent<Camera>();
	}

	protected virtual void OnEnable()
	{
		cam.SetReplacementShader(shader, replacementTag);

	}

	protected virtual void OnDisable()
	{
		cam.ResetReplacementShader();
	}
}
