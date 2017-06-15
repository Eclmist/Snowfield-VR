using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DualCamera : MonoBehaviour
{

	[SerializeField] private Camera secondaryCamera;

	private Vector3 secondaryCameraOffset;
	private Camera primaryCamera;
	//private RenderTexture hiddenCameraRT;

	void Start ()
	{
		primaryCamera = GetComponent<Camera>();
		UpdateSecondaryCameraSettings();

		//hiddenCameraRT = new RenderTexture(Screen.width, Screen.height, 24);
		//secondaryCamera.targetTexture = hiddenCameraRT;

		//Shader.SetGlobalTexture("_HiddenCameraTexture", hiddenCameraRT);
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
		secondaryCamera.hdr = primaryCamera.hdr;
		secondaryCamera.renderingPath = primaryCamera.renderingPath;
		secondaryCamera.fieldOfView = primaryCamera.fieldOfView;
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
