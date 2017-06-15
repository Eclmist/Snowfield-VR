using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VR_Controller_Custom : MonoBehaviour
{

    public enum Controller_Handle
    {
        LEFT,
        RIGHT
    }

    [SerializeField]
    private Controller_Handle handle;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private VR_Interactable_Object interactableObject;


    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    private void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        ControllerInput();
        
    }

    private void Update()
    {
        if (interactableObject != null)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                interactableObject.OnTriggerPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                interactableObject.OnTriggerRelease(this);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                interactableObject.OnGripPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
                interactableObject.OnGripRelease(this);
        }
    }

    private void ControllerInput()
    {
        if (interactableObject != null)
        {
            
            
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                interactableObject.OnTriggerHold(this);
            
            
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
                interactableObject.OnGripHold(this);
            
            if (interactableObject.LinkedController == this)
                interactableObject.OnInteracting(this);

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        VR_Interactable_Object currentObject = collider.GetComponentInParent<VR_Interactable_Object>();

        if (currentObject && (interactableObject == null || interactableObject.LinkedController != this))
        {
            interactableObject = currentObject;
            interactableObject.OnControllerEnter(this);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (interactableObject != null)
            interactableObject.OnControllerStay(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactableObject != null && interactableObject.LinkedController != this)
        {
            interactableObject = null;
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

    public void SetInteraction(VR_Interactable_Object _interacted)
    {
        interactableObject = _interacted;
    }

    public SteamVR_Controller.Device Device
    {
        get
        {
            return device;
        }
    }
}
