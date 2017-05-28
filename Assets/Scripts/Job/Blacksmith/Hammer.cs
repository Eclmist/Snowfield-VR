using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			ForgedBlade ingot = contact.otherCollider.GetComponentInParent<ForgedBlade>();

			if (ingot != null)
			{
				ingot.Impact(collision.relativeVelocity, contact.point);
			}
		}

	}

}
