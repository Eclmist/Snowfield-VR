using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VR_Interactable : MonoBehaviour {

	[SerializeField] public bool interactable = true;

	[Header("Vibrations")]
	[Range(0, 1)] protected float triggerEnterVibration = 0.4F;
	[Range(0, 1)] protected float triggerExitVibration = 0;
	[Range(0, 1)] protected float triggerPressVibration = 0.6F;

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
	{}

	protected virtual void OnControllerStay() { }

	protected virtual void OnControllerExit()
	{}

	protected virtual void OnTriggerPress()
	{
		Debug.Log("sdfsdsdfpresed");
		if (currentInteractingController)
		{
			
			currentInteractingController.Vibrate(triggerPressVibration);
		}
	}


	protected virtual void OnTriggerHold() { }

	protected virtual void OnTriggerRelease()
	{
		
	}

	protected virtual void Update()
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
