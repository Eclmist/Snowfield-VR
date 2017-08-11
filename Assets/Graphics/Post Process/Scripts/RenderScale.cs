using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class RenderScale : MonoBehaviour {

	public float resolutionScale = 1;

	// Use this for initialization
	void Start () {
		VRSettings.renderScale = resolutionScale;
	}

	// Update is called once per frame
	void Update () {
	}
}
