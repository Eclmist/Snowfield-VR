using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BlacksmithItem {

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

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
