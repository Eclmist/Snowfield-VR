using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActorUI : MonoBehaviour
{
	[SerializeField] private GameObject initiator;
	[SerializeField] private GameObject receiver;

	[SerializeField] private float distanceFromActor = 0.3F;
	[SerializeField] private float initiatorHeight = 1.5F;
	[SerializeField] private float receiverHeight = 1.5F;

	[SerializeField] private float rotationSpeed = 10;

	private Animator anim;

	protected void Start()
	{
		// Set starting position
		Vector3 targetPosition = initiator.transform.position +
			(receiver.transform.position- initiator.transform.position) * distanceFromActor;

		targetPosition.y = initiatorHeight;
		transform.position = targetPosition;

		// Set starting rotation
		Vector3 targetLookAt = initiator.transform.position;
		targetLookAt.y = transform.position.y;
		transform.LookAt(targetLookAt);

		// Get Anim
		anim = GetComponent<Animator>();
		if (anim == null)
			Destroy(this);
	}


	public void RotateToTarget()
	{
		StartCoroutine(Rotate());
	}

	private IEnumerator Rotate()
	{
		// Save starting transform state
		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

		// Calculate target point (rotation about pivot)
		Vector3 pivot = (initiator.transform.position + receiver.transform.position) / 2;
		pivot.y = startPos.y;

		// Calculate target local rotation
		Quaternion targetRot = 
			Quaternion.LookRotation(receiver.transform.position- initiator.transform.position, Vector3.up);

		for (float t = 0; t < 1; t += rotationSpeed)
		{
			Vector3 direction = startPos - pivot; // get point direction relative to pivot
			direction = Quaternion.Euler(new Vector3(0, t/1 * 180, 0)) * direction; // generate Rotation
			Vector3 targetPoint = direction + pivot; // calculate rotated point
			transform.position = targetPoint;

			transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
			yield return new WaitForFixedUpdate();
		}

		anim.SetTrigger("Open");
		Destroy(this);
	}
}
