using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLoader;
public class LoadSceneOnTriggerEnter : MonoBehaviour {

	public int sceneIndexToLoad;


	// Use this for initialization
	protected void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			LoadScene.Instance.Load(sceneIndexToLoad);
		}
	}

}
