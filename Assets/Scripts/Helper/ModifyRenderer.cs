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

	public void SetFloat(string shaderVar, float value, int index = 0)
	{
		foreach (Renderer ren in renderers)
		{
			if (index < ren.materials.Length)
				ren.materials[index].SetFloat(shaderVar, value);
		}
	}

	public void SetColor(string shaderVar, Color value, int index = 0)
	{
		foreach (Renderer ren in renderers)
		{
			if (index < ren.materials.Length)
				ren.materials[index].SetColor(shaderVar, value);
		}
	}

}
