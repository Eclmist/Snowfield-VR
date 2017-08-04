using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Glitch : RenderWithReplacement {

	[SerializeField] Material mat;

	protected void Update()
	{
		Shader.SetGlobalColor("_GlitchColor", mat.GetColor("_GlitchColor"));
		Shader.SetGlobalFloat("_GlitchSpeed", mat.GetFloat("_GlitchSpeed"));
		Shader.SetGlobalFloat("_MaskCutoff", mat.GetFloat("_MaskCutoff"));
		Shader.SetGlobalTexture("_Fractal", mat.GetTexture("_Fractal"));
		Shader.SetGlobalTexture("_Perlin", mat.GetTexture	("_Perlin"));

	}

}
