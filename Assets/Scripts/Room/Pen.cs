using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : GenericItem
{
	protected float pickupTime;
	protected float holdTime = 1;

	private VR_Controller_Custom controller;


	public override void OnTriggerPress(VR_Controller_Custom ctrl)
	{
		controller = ctrl;
		base.OnTriggerPress(ctrl);
		pickupTime = Time.time;
	}

	public override void OnFixedUpdateInteraction(VR_Controller_Custom ctrl)
	{
		base.OnFixedUpdateInteraction(ctrl);

		if (Time.time - pickupTime > holdTime)
		{
			OnTriggerRelease(controller);
			controller = null;
		}
	}
}
