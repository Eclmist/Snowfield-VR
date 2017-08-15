using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapper : MonoBehaviour {

	public Transform lockTransform;

	[SerializeField]
	protected VR_Interactable_Object snapItem;	// item to snap
	protected float snapDistance = .2f;

	protected bool isSnapped;   // current item snapped

	protected bool leftInitialPointFarEnough;

	void Start()
	{
		if (!snapItem || !lockTransform)
		{
			Destroy(this);
		}

		LockIntoPosition(snapItem.transform);
	}

	// Update is called once per frame
	void Update()
	{
		if (isSnapped)
		{
			if (snapItem.transform.position != lockTransform.position
						|| snapItem.transform.rotation != lockTransform.rotation)
			{
				isSnapped = false;
			}
		}
		else
		{
			if (Vector3.Distance(snapItem.transform.position, lockTransform.position) > 0.5F)
			{
				leftInitialPointFarEnough = true;
			}
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (!isSnapped && leftInitialPointFarEnough)
		{
			var i = other.GetComponentInParent<VR_Interactable_Object>();

			if (i != null)
			{
				if (i == snapItem)// && ingot.LinkedController == null)
				{

					var linkedController = i.LinkedController;

					if (linkedController)
					{
						linkedController.SetInteraction(null);
						i.LinkedController = null;
					}

					i.GetComponent<Rigidbody>().isKinematic = true;
					LockIntoPosition(i.transform);
					isSnapped = true;
				}
			}
		}
	}
	public void LockIntoPosition(Transform t)
	{
		t.position = lockTransform.position;
		t.rotation = lockTransform.rotation;
		leftInitialPointFarEnough = false;
	}

	//private void OnTriggerStay(Collider other)
	//{
	//	if (ingotReference == null)
	//	{
	//		Ingot ingot = other.GetComponent<Ingot>();

	//		if (ingot != null && ingot.LinkedController == null)
	//		{
	//			ingotReference = ingot;
	//			ingot.GetComponent<Rigidbody>().isKinematic = true;
	//			LockIntoPosition(ingot.transform);
	//		}
	//	}
	//}





}
