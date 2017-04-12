using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VR_Controller_Custom : MonoBehaviour {

    public enum Controller_Handle
    {
        LEFT,
        RIGHT
    }

    [SerializeField] private Controller_Handle handle;
    [SerializeField] private LayerMask interactableLayer;

    private SteamVR_TrackedObject trackedObject;
    private GameObject interactableObject;

    void Awake () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {
        ControllerInput();
	}

    private void ControllerInput()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
        if (interactableObject != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            IInteractable interactable = interactableObject.GetComponent<IInteractable>();
            interactable.Interact();
            switch (handle)
            {
                case Controller_Handle.LEFT:
                    Player.Instance.ChangeWield(EquipSlot.LEFTHAND, interactable);
                    break;
                case Controller_Handle.RIGHT:
                    Player.Instance.ChangeWield(EquipSlot.RIGHTHAND, interactable);
                    break;
            }
            interactableObject.transform.parent = transform;
            if (interactable.HasPivot)
            {
                interactableObject.transform.localPosition = interactable.PositionalOffset;
                interactableObject.transform.localRotation = Quaternion.Euler(interactable.RotationalOffset);
            }
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && interactableObject != null)
        {
            interactableObject.transform.parent = null;
            IInteractable interactable = interactableObject.GetComponent<IInteractable>();
            interactable.StopInteraction();
            switch (handle)
            {
                case Controller_Handle.LEFT:
                    Player.Instance.ChangeWield(EquipSlot.LEFTHAND, null);
                    break;
                case Controller_Handle.RIGHT:
                    Player.Instance.ChangeWield(EquipSlot.RIGHTHAND, null);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {          
        if(interactableObject == null && (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))))
        {
            Debug.Log("EnteredTriggerable");
            interactableObject = collider.gameObject;
        }else
        {
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        
        if (interactableObject != null && collider.gameObject == interactableObject)
        {
            Debug.Log("ExitTriggerable");
            interactableObject = null;
        }
        else
        {
            Debug.Log("Exited");
        }
    }
}
