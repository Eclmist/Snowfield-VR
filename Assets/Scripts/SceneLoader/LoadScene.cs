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
		private AsyncOperation uao;
		private bool coroutineStarted = false;


		void Awake()
		{
			DontDestroyOnLoad(gameObject);

			if (Instance != null)
				Destroy(this);
			else
				Instance = this;

		}

		//TODO: Samuel: Make some overloads for this function
		public void Load(int index)
		{
			StartCoroutine(LoadLevelAsync(index));
		}

		IEnumerator LoadLevelAsync(int index)
		{
			coroutineStarted = true;

			SteamVR_Fade.Start(Color.clear, 0);
			SteamVR_Fade.Start(Color.white, 2);

			ao = SceneManager.LoadSceneAsync(index);
			ao.allowSceneActivation = false;

			yield return new WaitForSeconds(3);
			ao.allowSceneActivation = true;

			while (!ao.isDone)
			{
				yield return null;
			}

			SteamVR_Fade.Start(Color.white, 0);
			SteamVR_Fade.Start(Color.clear, 2);
			coroutineStarted = false;
		}
	}
}