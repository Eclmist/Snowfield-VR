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
    [SerializeField]
    private LayerMask interactableLayer;
    private Rigidbody rigidBody;
    private SteamVR_TrackedObject trackedObject;
    private GameObject interactableObject;
    private IInteractable interacted;
    private SteamVR_Controller.Device device;

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControllerInput();
        UpdateInteractedObject();
    }


    private void UpdateInteractedObject()
    {
        if(interacted != null)
        interacted.UpdatePosition();
    }
    private void ControllerInput()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (interacted == null && interactableObject != null && (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)))
        {
            interacted = interactableObject.GetComponent<IInteractable>();
            interacted.Interact(this);


            switch (handle)
            {
                case Controller_Handle.LEFT:
                    Player.Instance.ChangeWield(EquipSlot.LEFTHAND, interacted);
                    break;
                case Controller_Handle.RIGHT:
                    Player.Instance.ChangeWield(EquipSlot.RIGHTHAND, interacted);
                    break;
            }



        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && interacted != null)
        {
            interacted.StopInteraction(this);
            interacted = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (interactableObject == null && (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))))
        {
            Debug.Log("EnteredTriggerable");
            interactableObject = collider.gameObject;
        }
        else
        {
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider collider)
    {

        if (interactableObject != null && collider.gameObject == interactableObject)
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
        if(val > 0 && val <= 10)
        {
            ushort passVal = (ushort) (val / 10 * 3999);
            if(passVal > 0)
            device.TriggerHapticPulse(passVal);
            Debug.Log("value: " + passVal);
        }
    }
}
