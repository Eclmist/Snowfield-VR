using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempKillScript : MonoBehaviour {


	public GameObject particle;

	void Kill()
	{
		particle.transform.parent = null;
		particle.SetActive(true);
		Destroy(gameObject, 0.1F);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			Kill();
	}
}
