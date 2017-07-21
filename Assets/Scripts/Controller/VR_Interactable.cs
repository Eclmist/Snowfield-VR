using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Interactable : MonoBehaviour {

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

	public virtual void OnControllerEnter(VR_Controller_Custom controller) {
	}

	public virtual void OnControllerStay(VR_Controller_Custom controller) { }

	public virtual void OnControllerExit(VR_Controller_Custom controller)
	{ }

	public virtual void OnTriggerPress(VR_Controller_Custom controller)
	{
		
	}

	public virtual void OnTriggerHold(VR_Controller_Custom controller) { }

	public virtual void OnTriggerRelease(VR_Controller_Custom controller)
	{
		
	}

	public virtual void OnGripPress(VR_Controller_Custom controller) { }

	public virtual void OnGripHold(VR_Controller_Custom controller) { }

	public virtual void OnGripRelease(VR_Controller_Custom controller) { }

	public virtual void OnInteracting(VR_Controller_Custom controller) {

		
	}
}
