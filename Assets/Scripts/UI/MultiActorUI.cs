using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActorUI : MonoBehaviour
{
	[SerializeField] private Transform initiator;
	[SerializeField] private Transform receiver;

    private float heightOffset = -0.25F;
    [SerializeField] private float receiverHeight = 1.5F;

	private float distanceFromActor = 0.5F;
	private float rotationSpeed = 1;

	private Animator anim;

	public bool disabled = true;

    public Transform Initiator
	{
		get { return initiator; }
		set { initiator = value; }
	}

	public Transform Receiver
	{
		get { return receiver; }
		set { receiver = value; }
	}

	public void Start()
	{
		// Get Anim
		anim = GetComponent<Animator>();
		if (anim == null)
			Destroy(this);
	}

	public void Initialize()
	{
		// Set starting position
		Vector3 targetPosition = initiator.transform.position +
			(receiver.transform.position - initiator.transform.position).normalized * distanceFromActor;

		targetPosition.y = Player.Instance.CurrentGroundHeight + Player.Instance.height + heightOffset;
		transform.position = targetPosition;

		// Set starting rotation
		Vector3 targetLookAt = initiator.position;
		targetLookAt.y = transform.position.y;
		transform.LookAt(targetLookAt);


		disabled = false;
	}


	public void RotateToTarget()
	{
		StartCoroutine(Rotate());
	}

	private IEnumerator Rotate()
	{
		if (!disabled)
		{
			yield return new WaitForSeconds(0.1F);

			// Save starting transform state
			Vector3 startPos = transform.position;
			Quaternion startRot = transform.rotation;

			// Calculate target point (rotation about pivot)
			Vector3 pivot = (initiator.position + receiver.position) / 2;
			pivot.y = startPos.y;

			// Calculate target local rotation

			Vector3 lookat = receiver.position - initiator.position;
			lookat.y = 0;

			Quaternion targetRot = Quaternion.LookRotation(lookat, Vector3.up);



			for (float t = 0; t <= 1; t += rotationSpeed * Time.deltaTime)
			{
				Vector3 targetPos = startPos.RotatePointAroundPivot(pivot, new Vector3(0, t/1 * 180, 0));
				transform.position = Vector3.Slerp(startPos, targetPos, t);

				transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
				yield return new WaitForFixedUpdate();
			}

			yield return new WaitForSeconds(0.1F);

		}
		anim.SetTrigger("Open");
		Destroy(this);
	}
}
