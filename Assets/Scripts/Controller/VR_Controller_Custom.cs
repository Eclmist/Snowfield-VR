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

	public Valve.VR.InteractionSystem.Hand Hand{
		get { return hand; }
	}

	public void SetModelActive(bool active)
	{
		foreach (SteamVR_RenderModel model in GetComponentsInChildren<SteamVR_RenderModel>()) {

			foreach (Renderer ren in model.GetComponentsInChildren<Renderer>()) 
				ren.enabled = active;
		}
	}

    private SteamVR_Controller.Device device;

    private VR_Interactable_Object interactable;

    private List<VR_Interactable_UI> listOfInteractingUI = new List<VR_Interactable_UI>();
	private List<VR_Interactable_Object> overlappedObjects = new List<VR_Interactable_Object>();

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
            Debug.Log(listOfInteractingUI.Count);
            return returnUI;
        }

    }

    public bool HasObject
    {
        get
        {
            return interactable != null;
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
		device = hand.controller;
        HandleFixedUpdateInput();
    }

    private void Update()
    {
        HandleUpdateInput();
	}

    private void HandleUpdateInput()
    {


        if (interactable != null && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && listOfInteractingUI.Count == 0)
            interactable.OnTriggerPress(this);
		if (interactable != null && interactable.LinkedController == this && device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
			interactable.OnTriggerRelease(this);
        if (interactable != null && interactable.LinkedController == this)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripRelease(this);

            interactable.OnUpdateInteraction(this);
        }
        listOfInteractingUI.RemoveAll(VR_Interactable_UI => VR_Interactable_UI == null || VR_Interactable_UI.LinkedController != this);
    }

    private void HandleFixedUpdateInput()
    {
        if (interactable != null && interactable.LinkedController == this)
        {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerHold(this);
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripHold(this);
            interactable.OnFixedUpdateInteraction(this);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
		VR_Interactable_Object currentObject = collider.GetComponentInParent<VR_Interactable_Object>();

        if (currentObject && (interactable == null || interactable.LinkedController != this))
        {
            if (interactable != null)
                interactable.OnControllerExit(this);
            interactable = currentObject;
            interactable.OnControllerEnter(this);
        }

        VR_Interactable_UI interactableUI = collider.GetComponent<VR_Interactable_UI>();

		if (interactableUI)
			listOfInteractingUI.Add (interactableUI);
	
		// Object list
		VR_Interactable_Object collided = collider.GetComponent<VR_Interactable_Object>();

		if (collided)
			overlappedObjects.Add (collided);
	}

//    private void OnTriggerStay(Collider collider)
//    {
//        if (interactable != null)
//			interactable.OnControllerStay(this);
//
//		VR_Interactable_Object closerObj = GetClosestInteractableObject ();

		//interactable = (closerObj.LinkedController == null) ? closerObj : interactable;
//    }

    public void OnTriggerExit(Collider other)
    {
        if (interactable != null && interactable.LinkedController != this)
        {
			interactable.OnControllerExit(this);
            interactable = null;
        }

        VR_Interactable_UI interactableUI = other.GetComponent<VR_Interactable_UI>();

        if (interactableUI)
            listOfInteractingUI.Remove(interactableUI);

		VR_Interactable_Object collided = GetComponent<Collider>().GetComponent<VR_Interactable_Object>();

		if (collided)
			overlappedObjects.Remove (collided);
    }

    public Vector3 Velocity
    {
        get { return device.velocity; }
    }

    public Vector3 AngularVelocity
    {
        get { return device.angularVelocity; }
    }

    public void Vibrate(float val)//pass in 1 - 10
    {
        if (val > 0 && val <= 10)
        {
            ushort passVal = (ushort)(val / 10 * 3999);
            if (passVal > 0)
                device.TriggerHapticPulse(passVal);
        }
    }

    public void SetInteraction(VR_Interactable_Object _interacted)
    {
        interactable = _interacted;
		if (interactable == null)
			SetModelActive (true);
    }

    public void Release()
    {
        interactable = null;
		SetModelActive (true);
    }

    public SteamVR_Controller.Device Device
    {
        get
        {
            return device;
        }
    }
}