using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeCamera : MonoBehaviour
{
	public static bool coroutineStarted;
	public static FadeCamera Instance;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		if (!Instance)
			Instance = this;
	}

	public static void Fade(Color targetColor, float targetOpacity, float fadetime = 1, UnityAction callback = null, Camera cam = null)
	{

		GameObject cameraObj;

		cameraObj = cam == null ? Camera.main.gameObject : cam.gameObject;

		Debug.Assert(cam);

		ColorOverlay script = cameraObj.GetComponent<ColorOverlay>();
		if (!script)
			script = cameraObj.AddComponent<ColorOverlay>();

		Debug.Assert(script);


		if (!coroutineStarted)
		{
			Instance.StartCoroutine(Instance.FadeCoroutine(targetColor, targetOpacity, fadetime, script, callback));
		}
	}

	IEnumerator FadeCoroutine(Color targetColor, float targetOpacity, float fadetime, ColorOverlay script, UnityAction callback)
	{
		float currentOpacity = script.GetOpacity();
		script.SetColor(targetColor);

		float startTime = Time.time;
		while (currentOpacity < targetOpacity)
		{
			currentOpacity = Mathf.Clamp01((Time.time - startTime) / fadetime);
			script.SetOpacity(currentOpacity);
			yield return null;
		}

		callback.Invoke();
	}
}
