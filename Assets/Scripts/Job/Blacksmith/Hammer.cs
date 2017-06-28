using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BlacksmithItem {

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);

		IngotDeformer ingotDeformer = collision.collider.GetComponentInParent<IngotDeformer>();

		if (ingotDeformer != null)
		{
			foreach (ContactPoint contact in collision.contacts)
			{
				ingotDeformer.Impact(collision.relativeVelocity, contact.point);
			}

			// Ingot.IncrementMorphStep();
		}

	}

}
