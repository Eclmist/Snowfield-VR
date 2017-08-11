using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Controller_Custom : MonoBehaviour
{

	public enum Controller_Handle
	{
		LEFT,
		RIGHT
	}

	[SerializeField]
	private Controller_Handle handle;
	private Valve.VR.InteractionSystem.Hand hand;

	public Valve.VR.InteractionSystem.Hand Hand
	{
		get { return hand; }
	}

	public void SetModelActive(bool active)
	{
		foreach (SteamVR_RenderModel model in GetComponentsInChildren<SteamVR_RenderModel>())
		{

			foreach (Renderer ren in model.GetComponentsInChildren<Renderer>())
				ren.enabled = active;
		}
	}

	private SteamVR_Controller.Device device;

	private VR_Interactable_Object overlappedInteractableObject;

	private List<VR_Interactable_UI> listOfInteractingUI = new List<VR_Interactable_UI>();

	public VR_Interactable_UI UI
	{
		get
		{

			float closestDist = float.MaxValue;
			VR_Interactable_UI returnUI = null;
			foreach (VR_Interactable_UI ui in listOfInteractingUI)
			{
				float dist = Vector3.Distance(transform.position, ui.transform.position);
				if (dist < closestDist)
				{
					closestDist = dist;
					returnUI = ui;
				}
			}
			return returnUI;
		}

	}


	public VR_Interactable_Object CurrentItemInHand
	{
		get
		{
			return overlappedInteractableObject;
		}
	}
	public Controller_Handle Handle
	{
		get
		{
			return handle;
		}
	}

	//	public VR_Interactable_Object GetClosestInteractableObject()
	//	{
	//		float distance = float.MaxValue;
	//		VR_Interactable_Object closestObj = null;
	//
	//		foreach (VR_Interactable_Object o in overlappedObjects) {
	//			float distanceToCtrl = Vector3.Distance (transform.position, o.transform.position);
	//
	//			if (distanceToCtrl < distance) {
	//				distance = distanceToCtrl;
	//				closestObj = o;
	//			}
	//		}
	//
	//		return closestObj;
	//	}

	void Awake()
	{
		hand = GetComponent<Valve.VR.InteractionSystem.Hand>();
	}

	private void FixedUpdate()
	{
		SteamVR_Controller.Device checkDevice = hand.controller;
		if (checkDevice != null)
			device = checkDevice;
		HandleFixedUpdateInput();
	}

	private void Update()
	{
		HandleUpdateInput();

		if (overlappedInteractableObject == null || overlappedInteractableObject.LinkedController != this) //if controller not already held on to some object
		{
			Collider[] overlappedColliders = Physics.OverlapSphere(hand.hoverSphereTransform.position, hand.hoverSphereRadius, LayerMask.GetMask("Interactable", "Player"));

			if (overlappedColliders.Length > 0)
			{
				float closestObjDist = float.MaxValue;
				VR_Interactable_Object obj = null;

				foreach (Collider c in overlappedColliders)
				{
					VR_Interactable_Object o = c.GetComponentInParent<VR_Interactable_Object>();

					if (o)
					{
						float distance = Vector3.Distance(hand.hoverSphereTransform.position, c.transform.position);
						if (distance < closestObjDist)
						{
							closestObjDist = distance;
							obj = o;
						}
					}
				}

				if (obj && obj != overlappedInteractableObject)
				{
					if (overlappedInteractableObject)
						overlappedInteractableObject.OnControllerExit(this);

					overlappedInteractableObject = obj;
					overlappedInteractableObject.OnControllerEnter(this);
				}
			}
			else
			{
				if (overlappedInteractableObject)
				{
					overlappedInteractableObject.OnControllerExit(this);
					overlappedInteractableObject = null;
				}
			}
		}
	}

	private void HandleUpdateInput()
	{


		if (overlappedInteractableObject != null && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && listOfInteractingUI.Count == 0)
			overlappedInteractableObject.OnTriggerPress(this);
		if (overlappedInteractableObject != null && overlappedInteractableObject.LinkedController == this && device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			overlappedInteractableObject.OnTriggerRelease(this);
		if (overlappedInteractableObject != null && overlappedInteractableObject.LinkedController == this)
		{
			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
				overlappedInteractableObject.OnGripPress(this);
			if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
				overlappedInteractableObject.OnGripRelease(this);

			overlappedInteractableObject.OnUpdateInteraction(this);
		}
		listOfInteractingUI.RemoveAll(VR_Interactable_UI => VR_Interactable_UI == null || VR_Interactable_UI.LinkedController != this);
	}

	private void HandleFixedUpdateInput()
	{
		if (overlappedInteractableObject != null && overlappedInteractableObject.LinkedController == this)
		{
			if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
				overlappedInteractableObject.OnTriggerHold(this);
			if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
				overlappedInteractableObject.OnGripHold(this);

			overlappedInteractableObject.OnFixedUpdateInteraction(this);
		}
	}

	public void OnTriggerEnter(Collider collider)
	{

		VR_Interactable_UI interactableUI = collider.GetComponent<VR_Interactable_UI>();

		if (interactableUI)
			listOfInteractingUI.Add(interactableUI);

	}


	public void OnTriggerExit(Collider other)
	{
		if (overlappedInteractableObject != null && overlappedInteractableObject.LinkedController != this)
		{
			overlappedInteractableObject.OnControllerExit(this);
			overlappedInteractableObject = null;
		}

		VR_Interactable_UI interactableUI = other.GetComponent<VR_Interactable_UI>();

		if (interactableUI)
			listOfInteractingUI.Remove(interactableUI);
	}

	public Vector3 Velocity
	{
		get { return device.velocity; }
	}

	public Vector3 AngularVelocity
	{
		get { return device.angularVelocity; }
	}

	public void Vibrate(float val = 1)
	{
		float normalizedValue = val;
		val *= 10; //Normalizing to 0-10

		val = Mathf.Max(val, 0);
		val = Mathf.Min(val, 1);

		if (val > 0)
		{
			if (device != null)
				device.TriggerHapticPulse((ushort)(val * 3999));
		}
	}

	public void SetInteraction(VR_Interactable_Object _interacted)
	{
		overlappedInteractableObject = _interacted;
		if (overlappedInteractableObject == null)
			SetModelActive(true);
	}

	public void Release()
	{
		overlappedInteractableObject = null;
		SetModelActive(true);
	}

	public SteamVR_Controller.Device Device
	{
		get
		{
			return device;
		}
	}
}