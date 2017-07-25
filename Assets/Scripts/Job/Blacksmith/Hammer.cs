using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BlacksmithItem
{
	private AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);


		IngotDeformer ingotDeformer = collision.collider.GetComponentInParent<IngotDeformer>();
		if (ingotDeformer != null)
		{
            if (ingotDeformer.enabled)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    ingotDeformer.Impact(collision.relativeVelocity, contact.point);
                }

                collision.collider.GetComponent<Ingot>().IncrementMorphStep();
            }
		}

		if (source)
			source.PlayOneShot(source.clip, collision.relativeVelocity.magnitude / 3);



	}

}
