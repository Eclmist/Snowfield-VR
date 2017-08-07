using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoader
{

	public class LoadScene : MonoBehaviour
	{
		public static LoadScene Instance;

		private AsyncOperation ao;
		private bool coroutineStarted = false;


		void Awake()
		{
			DontDestroyOnLoad(gameObject);

			if (Instance != null)
				Destroy(this);
			else
				Instance = this;

		}

		private int targetIndex;

		//TODO: Samuel: Make some overloads for this function
		public void Load(int index)
		{
			targetIndex = index;
			FadeCamera.Fade(Color.white, 1, 1, LoadCallback);
		}

		private void LoadCallback()
		{
			// Load the loading screen scene
			SceneManager.LoadSceneAsync(1);

			if (!coroutineStarted)
				StartCoroutine(LoadLevelAsync(targetIndex));
		}

		IEnumerator LoadLevelAsync(int index)
		{
			coroutineStarted = true;

			ao = SceneManager.LoadSceneAsync(index);
			ao.allowSceneActivation = false;

			while (!ao.isDone)
			{
				LoadingScreen.progress = ao.progress;

				if (ao.progress >= 0.9F)
				{
					if (Input.anyKeyDown)
					ao.allowSceneActivation = true;
				
				}

				yield return null;
			}

			coroutineStarted = false;
		}
	}
}