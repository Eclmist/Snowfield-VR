using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphingTable : MonoBehaviour
{

	public Transform lockTransform;

	[SerializeField]
	private Ingot ingotReference;

	void Start()
	{
	}


	// Update is called once per frame
	void Update()
	{

		if (ingotReference != null)
		{
			if (ingotReference.transform.position != lockTransform.position
						|| ingotReference.transform.rotation != lockTransform.rotation)
			{
				ingotReference.OnAnvil = false;
				ingotReference = null;

			}

		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (ingotReference == null)
		{
			Ingot ingot = other.gameObject.GetComponent<Ingot>();

			if (ingot != null && ingot.LinkedController == null)
			{
				ingotReference = ingot;
				ingot.GetComponent<Rigidbody>().isKinematic = true;
				LockIntoPosition(ingot.transform);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (ingotReference == null)
		{
			Ingot ingot = other.GetComponent<Ingot>();

			if (ingot != null && ingot.LinkedController == null)
			{
				ingotReference = ingot;
				ingot.GetComponent<Rigidbody>().isKinematic = true;
				LockIntoPosition(ingot.transform);
			}
		}
	}

	


	public void LockIntoPosition(Transform t)
	{
		ingotReference.OnAnvil = true;
		t.position = lockTransform.position;
		t.rotation = lockTransform.rotation;

	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(GetComponent<SphereCollider>().center, GetComponent<SphereCollider>().radius);
	}












}
