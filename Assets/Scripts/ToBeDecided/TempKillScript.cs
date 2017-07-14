using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempKillScript : MonoBehaviour {


	public GameObject particle;

	public void Kill()
	{
		particle.transform.parent = null;
		particle.SetActive(true);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			Kill();
	}
}
