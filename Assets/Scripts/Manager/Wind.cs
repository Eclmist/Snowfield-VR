using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Wind : MonoBehaviour
{
	public Vector4 wind;
	public float windFrequency;

	protected void Start ()
	{
		Shader.SetGlobalColor("_Wind", wind);

	}

	protected void Update ()
	{
		Vector4 WindRGBA = wind * Mathf.Sin(Time.realtimeSinceStartup * windFrequency);
		WindRGBA.w = wind.w;
		Shader.SetGlobalColor("_Wind", WindRGBA);

	}
}
