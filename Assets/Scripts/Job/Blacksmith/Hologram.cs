using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{

	[SerializeField] private Material holographicMat;

	private Renderer[] childRenderers;

	// Use this for initialization
	protected void Start () {
		childRenderers = GetComponentsInChildren<Renderer>();

		//Replace all renderer material with holographic material;
		foreach (Renderer ren in childRenderers)
		{
			ren.material = holographicMat;
		}
	}
	
	// Update is called once per frame
	protected void Update () {
		
	}
}
