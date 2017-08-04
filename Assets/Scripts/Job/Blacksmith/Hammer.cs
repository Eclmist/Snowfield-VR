using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BlacksmithItem
{
	private AudioSource source;

	private float timeSinceLastHit = 0;

	void Start()
	{
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		timeSinceLastHit += Time.deltaTime;
	}



	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);


		IngotDeformer ingotDeformer = collision.collider.GetComponentInParent<IngotDeformer>();
		if (ingotDeformer != null && timeSinceLastHit > 0.33F)
		{
			if (ingotDeformer.enabled)
			{
				foreach (ContactPoint contact in collision.contacts)
				{
					ingotDeformer.Impact(collision.relativeVelocity, contact.point);
					break;
				}

				collision.collider.GetComponent<Ingot>().IncrementMorphStep();
			}

			if (source)
				source.PlayOneShot(source.clip, collision.relativeVelocity.magnitude / 3);

			Debug.Log("Hit On Collision enter" + collision.gameObject.name);

			timeSinceLastHit = 0;

		}


	}

}
