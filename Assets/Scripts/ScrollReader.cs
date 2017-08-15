using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollReader : MonoBehaviour
{

	public Transform lockTransform;
	public Transform trailTargetPosition;
	public Transform pageflipPosition;
	public Text name;

	private MagicScroll scrollReference;
	private GameObject gestureTrailObjectReference;
	public GameObject onePageFlip;
	private GameObject flipRef;
	void Start()
	{
		name.text = "";
	}


	// Update is called once per frame
	void Update()
	{

		if (scrollReference != null)
		{
			if (scrollReference.transform.position != lockTransform.position
						|| scrollReference.transform.rotation != lockTransform.rotation)
			{
				Destroy(gestureTrailObjectReference);
				Destroy(flipRef);
				scrollReference = null;
				name.text = "";

			}

		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (scrollReference == null)
		{
			MagicScroll scroll = other.gameObject.GetComponent<MagicScroll>();

			if (scroll != null)// && ingot.LinkedController == null)
			{
				var linkedController = scroll.LinkedController;

				if (linkedController)
				{
					linkedController.SetInteraction(null);
					scroll.LinkedController = null;
				}

				scrollReference = scroll;
				scroll.GetComponent<Rigidbody>().isKinematic = true;
				LockIntoPosition(scroll.transform);

				gestureTrailObjectReference = 
					Instantiate(scroll.trailGestureObject, trailTargetPosition.position, trailTargetPosition.rotation);

				flipRef = Instantiate(onePageFlip, pageflipPosition.position, pageflipPosition.rotation);

				name.text = scroll.name;
			}
		}
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




	public void LockIntoPosition(Transform t)
	{
		t.position = lockTransform.position;
		t.rotation = lockTransform.rotation;
	}
}
