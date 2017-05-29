using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempKillScript : MonoBehaviour {


	public GameObject particle;

	void Kill()
	{
		particle.transform.parent = null;
		particle.SetActive(true);
		gameObject.SetActive(false);
		Destroy(gameObject, 1);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			Kill();
	}
}
