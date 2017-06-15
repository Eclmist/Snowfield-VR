/*
 * Base class for world space interactable VR UI.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VR_Interactable : MonoBehaviour
{
	[SerializeField] public bool interactable = true;

	[Header("Vibrations")]
	[SerializeField] [Range(0, 10)] protected float triggerEnterVibration = 0.8F;
	[SerializeField] [Range(0, 10)] protected float triggerExitVibration = 0.3F;
	[SerializeField] [Range(0, 10)] protected float triggerPressVibration = 0;

	protected virtual void OnControllerEnter()
	{
		currentInteractingController.Vibrate(triggerEnterVibration);
	}
	protected virtual void OnControllerStay() { }
	protected virtual void OnControllerExit()
	{
		currentInteractingController.Vibrate(triggerExitVibration);
        currentInteractingController = null;
    }

    protected virtual void OnTriggerPress()
	{
		currentInteractingController.Vibrate(triggerPressVibration);
	}
	protected virtual void OnTriggerHold() { }
	protected virtual void OnTriggerRelease() { }

    protected virtual void OnApplicationMenuPress()
    {
        currentInteractingController.Vibrate(triggerPressVibration);
        Debug.Log("App");
    }

	protected virtual void OnInteractableChange() { }

	protected VR_Controller_Custom currentInteractingController;

	protected void OnTriggerEnter(Collider other)
	{
		if (interactable)
		{
			VR_Controller_Custom vrController = other.GetComponent<VR_Controller_Custom>();

			if (vrController && currentInteractingController == null)
			{
				currentInteractingController = vrController;
				OnControllerEnter();
			}
		}
	}

	protected void OnTriggerStay(Collider other)
	{
		if (interactable)
		{

			VR_Controller_Custom vrController = other.GetComponent<VR_Controller_Custom>();

			if (vrController && currentInteractingController == vrController)
			{
				OnControllerStay();

                if (vrController.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                    OnTriggerPress();
                else if (vrController.Device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    OnTriggerHold();
                else if (vrController.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                    OnTriggerRelease();
                else if (vrController.Device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                    OnApplicationMenuPress();

			}
		}
	}

	protected void OnTriggerExit(Collider other)
	{
		if (interactable)
		{

			VR_Controller_Custom vrController = other.GetComponent<VR_Controller_Custom>();

			if (vrController && currentInteractingController == vrController)
			{
                OnControllerExit();
			}
		}
	}

	private bool lastInteractable;

	protected void Update()
	{
		if (lastInteractable != interactable)
		{
			lastInteractable = interactable;
			OnInteractableChange();
		}
	}

}
