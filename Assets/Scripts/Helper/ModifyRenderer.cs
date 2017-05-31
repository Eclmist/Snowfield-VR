using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyRenderer : MonoBehaviour {

	[SerializeField] private Renderer[] renderers;

	// Use this for initialization
	protected void Start ()
	{
		if (renderers.Length == 0)
		{
			Destroy(this);

		}
	}

	public void SetFloat(string shaderVar, float value)
	{
		foreach (Renderer ren in renderers)
		{
			ren.material.SetFloat(shaderVar, value);
		}
	}

	public void SetColor(string shaderVar, Color value)
	{
		foreach (Renderer ren in renderers)
		{
			ren.material.SetColor(shaderVar, value);
		}
	}

}
