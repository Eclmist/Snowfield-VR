using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VR_Controller_Custom : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    public GameObject Model
    {
        get
        {
            return model;
        }
    }

    public enum Controller_Handle
    {
        LEFT,
        RIGHT
    }

    [SerializeField]
    private Controller_Handle handle;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private VR_Interactable interactable;

    private VR_Interactable_UI currentInteractingUI;

    public VR_Interactable_UI UI
    {
        get
        {
            return currentInteractingUI;
        }
        set
        {
            currentInteractingUI = value;
        }
    }

    public Controller_Handle Handle
    {
        get
        {
            return handle;
        }
    }

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        model = transform.Find("Model").gameObject;
    }

    private void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        ControllerInput();

    }

    private void Update()
    {
        if (interactable != null)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerRelease(this);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripRelease(this);
        }
    }

    private void ControllerInput()
    {
        if (interactable != null)
        {


            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerHold(this);
            
            
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripHold(this);
            
            if (interactable.LinkedController == this)
                interactable.OnInteracting(this);

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        VR_Interactable currentObject = collider.GetComponentInParent<VR_Interactable>();

        if (currentObject && (interactable == null || interactable.LinkedController != this))
        {
            if (interactable != null)
                interactable.OnControllerExit(this);
            interactable = currentObject;
            interactable.OnControllerEnter(this);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (interactable != null)
            interactable.OnControllerStay(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactable != null && interactable.LinkedController != this)
        {
            interactable.OnControllerExit(this);
            interactable = null;
		}
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

    public void SetInteraction(VR_Interactable _interacted)
    {
        interactable = _interacted;
    }

    public SteamVR_Controller.Device Device
    {
        get
        {
            return device;
        }
    }
}