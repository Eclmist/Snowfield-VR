using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCol : MonoBehaviour {

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	protected void OnCollisionEnter(Collision col)
	{
		Debug.Log("Entered: " + col.gameObject.name);
	}

	protected void OnCollisionStay(Collision col)
	{
		Debug.Log("Stay: " + col.gameObject.name);
	}


	protected void OnCollisionExit(Collision col)
	{
		Debug.Log("Exit: " + col.gameObject.name);
	}

}
