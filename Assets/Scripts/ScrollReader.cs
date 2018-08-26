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
				StopAllCoroutines();
				StartCoroutine(ShowNewSpell(scroll));
			}
		}
	}

	IEnumerator ShowNewSpell(MagicScroll scroll)
	{

		flipRef = Instantiate(onePageFlip, pageflipPosition.position, pageflipPosition.rotation);
		name.CrossFadeAlpha(0, 0, true);
		name.CrossFadeAlpha(1, 0.75F, false);
		name.text = scroll._Name;

		yield return new WaitForSeconds(0.75F);

		gestureTrailObjectReference =
Instantiate(scroll.trailGestureObject, trailTargetPosition.position, trailTargetPosition.rotation);

		//float val = 0;

		//TrailRenderer[] trailRenderers = gestureTrailObjectReference.GetComponentsInChildren<TrailRenderer>();
		//while (val < 1)
		//{
		//	val += Time.deltaTime;

		//	foreach (TrailRenderer t in trailRenderers)
		//	{
		//		t.widthMultiplier = val;
		//		yield return null;
		//	}
		//}
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
