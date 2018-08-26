using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class ColorOverlay : GenericPostProcess {

	[SerializeField] protected Color color;
	[SerializeField] [Range(0, 1)] protected float opacity;

	protected override void Start()
	{
		shader = Shader.Find("Hidden/ColorOverlay");
		base.Start();
	}

	protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		mat.SetColor("_Color", color);
		mat.SetFloat("_Opacity", opacity);

		base.OnRenderImage(src, dst);
	}

	public void SetOpacity(float o)
	{
		opacity = o;
	}

	public float GetOpacity()
	{
		return opacity;
	}

	public void SetColor(Color c)
	{
		color = c;
	}
}
