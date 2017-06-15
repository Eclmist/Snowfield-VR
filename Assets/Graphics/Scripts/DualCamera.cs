using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DualCamera : MonoBehaviour
{
	[SerializeField] private Camera primaryCamera;
	[SerializeField] private Camera secondaryCamera;
	[SerializeField] private Vector3 secondaryCameraOffset;
	private RenderTexture hiddenCameraRT;

	void Awake ()
	{
		UpdateSecondaryCameraSettings();

		hiddenCameraRT = new RenderTexture(Screen.width, Screen.height, 24);
		secondaryCamera.targetTexture = hiddenCameraRT;

		Shader.SetGlobalTexture("_SecondaryCameraTex", hiddenCameraRT);
	}

	void Update ()
	{
		secondaryCamera.fieldOfView = primaryCamera.fieldOfView;
		secondaryCamera.transform.position = transform.position + secondaryCameraOffset;
		secondaryCamera.transform.rotation = transform.rotation;
	}

	private void UpdateSecondaryCameraSettings()
	{
		secondaryCamera.nearClipPlane = primaryCamera.nearClipPlane;
		secondaryCamera.farClipPlane = primaryCamera.farClipPlane;
		secondaryCamera.allowHDR = primaryCamera.allowHDR;
		secondaryCamera.allowMSAA = primaryCamera.allowMSAA;
		secondaryCamera.renderingPath = primaryCamera.renderingPath;
		secondaryCamera.fieldOfView = primaryCamera.fieldOfView;
		secondaryCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left,
			primaryCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left));
		secondaryCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right,
			primaryCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));

	}

	public void SwapCamera()
	{
		secondaryCameraOffset *= -1;
	}

	public void SetOffset(Vector3 offset)
	{
		secondaryCameraOffset = offset;
	}
}
