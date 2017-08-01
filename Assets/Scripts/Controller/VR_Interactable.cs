using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Interactable : MonoBehaviour {

	[SerializeField] public bool interactable = true;

	[Header("Vibrations")]
	[SerializeField] [Range(0, 10)] protected float triggerEnterVibration = 1;
	[SerializeField] [Range(0, 10)] protected float triggerExitVibration = 1;
	[SerializeField] [Range(0, 10)] protected float triggerPressVibration =0.75f;

	protected VR_Controller_Custom currentInteractingController;

	protected virtual void Awake(){
	}
	protected virtual void Start(){}

	public VR_Controller_Custom LinkedController
	{
		get
		{
			return currentInteractingController;
		}
		set
		{
			currentInteractingController = value;
		}
	}

	protected virtual void OnControllerEnter()
	{
			if (currentInteractingController)
			currentInteractingController.Vibrate(triggerEnterVibration);

	}


	protected virtual void OnControllerStay() { }

	protected virtual void OnControllerExit()
	{
		if (currentInteractingController)
			currentInteractingController.Vibrate(triggerExitVibration);
	}

	protected virtual void OnTriggerPress()
	{
		if (currentInteractingController)
			currentInteractingController.Vibrate(triggerPressVibration);
	}


	protected virtual void OnTriggerHold() { }

	protected virtual void OnTriggerRelease()
	{
		
	}

	protected virtual void OnGripPress() { }

	protected virtual void OnGripHold() { }

	protected virtual void OnGripRelease() { }

	protected virtual void OnFixedUpdateInteraction() { }

	protected virtual void OnInteractableChange() {
	}



	// UNITY FUNCTIONS




}
