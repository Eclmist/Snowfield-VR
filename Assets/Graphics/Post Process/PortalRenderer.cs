using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRenderer : MonoBehaviour
{

	protected void Awake()
	{
		RenderTexture portalTexture = new RenderTexture(Screen.width, Screen.height, 24);

	}

	protected void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest);
	}

}
