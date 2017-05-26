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
    private IInteractable interactableObject;
    private SteamVR_Controller.Device device;

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        
    }

    private void Start()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
    }
    // Update is called once per frame
    void Update()
    {
        ControllerInput();
    }

    public SteamVR_Controller.Device Device
    {
        get
        {
            return device;
        }
    }

    private void ControllerInput()
    {
        //Debug.Log(interactableObject);
        if (interactableObject != null)
        {
            interactableObject.Interact(this);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        IInteractable interacted = collider.GetComponentInParent<IInteractable>();
        if (interacted != null && (interactableObject == null || interactableObject.LinkedController == null))
        {
            interactableObject = interacted;
            if(interactableObject != null)
            Vibrate(5f);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        IInteractable interacted = collider.GetComponentInParent<IInteractable>();
        if (interacted != null && (interactableObject == null || interactableObject.LinkedController == null))
        {
            interactableObject = interacted;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(interactableObject != null && interactableObject.LinkedController != this)
        {
            interactableObject = null;
        }
    }

    public Vector3 Velocity()
    {
        return device.velocity;
    }

    public Vector3 AngularVelocity()
    {
        return device.angularVelocity;
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

    public void SetInteraction(IInteractable _interacted)
    {
        interactableObject = _interacted;
    }
}
