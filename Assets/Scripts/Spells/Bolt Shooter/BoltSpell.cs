using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpell/* : Spell*/ {

    [SerializeField]
    private int castCount;

 //   protected override void Update()
 //   {

	//	//Debug.Log("castcount : " + castCount);

	//	if (castCount <= 0)
	//	{
	//		var em = Indicator.GetComponent<ParticleSystem>().emission;
	//		em.enabled = false;
	//		Destroy(Indicator, 1);
	//	}
	//}

	////protected override void Start()
	////{
	////	transform.parent = null;
	////	currentInteractingController.SetInteraction(null);
	////}

	//protected override void OnTriggerPress()
 //   {


	//	//if (castCount > 0)
	//	//{
	//	//	if (spellPrefab != null)
	//	//	{
	//	//		Instantiate(spellPrefab, currentInteractingController.transform).transform.parent = null;
	//	//		castCount--;

	//	//	}


	//	//}
	//	Instantiate(spellPrefab, currentInteractingController.transform).transform.parent = null;
	//}

}
