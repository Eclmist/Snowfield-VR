/*
 * Base class for world space interactable VR UI.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VR_Interactable_UI : VR_Interactable
{

	private bool lastInteractable;

	protected virtual void Update()
	{
	
		if (lastInteractable != interactable)
		{
			lastInteractable = interactable;
			OnInteractableChange();
		}
		if (currentInteractingController)
		{
			if (currentInteractingController.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				OnTriggerPress();
			}
			else if (currentInteractingController.Device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
				OnTriggerHold();
			else if (currentInteractingController.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
				OnTriggerRelease();
		}


	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (interactable)
		{
			VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();
			
			if (vrController && currentInteractingController == null)
			{
				currentInteractingController = vrController;
				OnControllerEnter();
			}
		}
	}

	protected virtual void OnTriggerStay(Collider other)
	{
		if (interactable)
		{

			VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();

			if (vrController && currentInteractingController == vrController)
			{
				OnControllerStay();



			}
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (interactable)
		{

			VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();

			if (vrController && currentInteractingController == vrController)
			{
				OnControllerExit();
			}
		}
	}

	protected override void OnControllerEnter()
	{
		base.OnControllerEnter();

		if (currentInteractingController)
			currentInteractingController.Vibrate(triggerEnterVibration);

	}

	protected override void OnControllerExit()
	{
		base.OnControllerExit();

		if (currentInteractingController)
			currentInteractingController.Vibrate(triggerExitVibration);

		currentInteractingController = null;


	}
}


