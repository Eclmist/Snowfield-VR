using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_ScrollRect : VR_Interactable_UI {

	[SerializeField] private ScrollRect sr;
	[SerializeField][Range(0,100000)] private float scrollSensitivity = 2;
	[SerializeField][Range(0,100)] private float velocityMultiplier = 3;

	private Vector3 initialPos;
	private float initialSRPos;
	private RectTransform child;

	private float rectWorldHeight= 0;
	// Use this for initialization
	void Start () {
		child = sr.content.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {

		base.Update ();

		if (Input.GetKeyDown (KeyCode.UpArrow))
			sr.verticalNormalizedPosition = 1.3f;

	}

	protected override void OnTriggerPress ()
	{
		base.OnTriggerPress ();
		initialPos = currentInteractingController.transform.position;
		initialSRPos = sr.verticalNormalizedPosition;

	}

	protected override void OnTriggerHold ()
	{
		base.OnTriggerHold ();

		float targetPos = initialSRPos + (initialPos.y - currentInteractingController.transform.position.y) / rectWorldHeight;

		sr.verticalNormalizedPosition = targetPos; // * scrollSensitivity;


	}

	protected override void OnTriggerRelease ()
	{
		base.OnTriggerRelease ();
		sr.velocity = new Vector2 (0, currentInteractingController.Velocity.y * velocityMultiplier * 1000);

	}

	protected override void OnControllerStay()
	{
		Vector3 worldTop = child.TransformPoint(child.rect.yMin, 0, 0);
		Vector3 worldBtm = child.TransformPoint(child.rect.yMax, 0, 0);

		rectWorldHeight = (worldTop - worldBtm).magnitude;
	}

	protected override void OnControllerExit()
	{
		sr.velocity = new Vector2 (0, currentInteractingController.Velocity.y * velocityMultiplier * 1000);
		base.OnControllerExit ();

	}


}
