using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class FlameChargeSpell : Spell
//{
//    protected bool hasFlag = false;
//	protected bool casted = false;

//	[SerializeField]
//	protected GameObject 

//	protected override void OnTriggerHold()
//	{
//		Debug.Log("I'm Holding in Charge");

//		base.OnTriggerHold();

//		if (!casted)
//		{
//			if (!hasFlag)
//			{
//				spellGO = Instantiate(spellPrefab, currentInteractingController.transform);

//				hasFlag = true;
//			}
//			else
//			{
//				var em = spellGO.GetComponent<ParticleSystem>().emission;

//				var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

//				em.enabled = true;
//				emsmoke.enabled = true;

//				casted = true;
//			}
//		}
//	}

//	protected override void OnTriggerRelease()
//	{
//		if (casted)
//		{
//			var em = spellGO.GetComponent<ParticleSystem>().emission;

//			var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

//			var emindicator = indicator.GetComponent<ParticleSystem>().emission;

//			emindicator.enabled = false;
//			em.enabled = false;
//			emsmoke.enabled = false;

//			Destroy(indicator, 0.1f);
//			Destroy(spellGO, 0.5f);

//			casted = false;
//		}

//	}
//}