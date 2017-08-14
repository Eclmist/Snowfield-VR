using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BlacksmithItem
{
	private AudioSource source;

	private float timeSinceLastHit = 0;
	private Vector3 originalPosition, originalRotation;
	protected override void Start()
	{
        base.Start();
		source = GetComponent<AudioSource>();
		originalPosition = transform.position;
		originalRotation = transform.rotation.eulerAngles;
	}

	protected override void Update()
	{
		base.Update();
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


			timeSinceLastHit = 0;

		}
	}

	public override void OnTriggerPress(VR_Controller_Custom controller)
	{
		if (controller != LinkedController)
		{
			base.OnTriggerPress(controller);
			targetPositionPoint.transform.position = controller.transform.position;
			targetPositionPoint.transform.rotation = controller.transform.rotation;
		}
	}

	public override void OnTriggerRelease(VR_Controller_Custom referenceCheck)
	{
	}
}
