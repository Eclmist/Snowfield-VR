using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Windzone : MonoBehaviour
{
	[SerializeField]
	public float force = 10;
	private float range = 10;
	public SphereCollider collider;

	void OnEnable()
	{
		range = collider.radius;
	}

	void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();

		if (rb && !rb.isKinematic)
		{
			rb.AddExplosionForce(force, transform.position, range);
		}
	}

}
