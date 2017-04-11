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

        
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
        if (interactableObject != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("gripped");
            switch (handle)
            {
                case Controller_Handle.LEFT:
                    Player.Instance.ChangeWield(EquipSlot.LEFTHAND, interactableObject.GetComponent<IInteractable>());
                    break;
                case Controller_Handle.RIGHT:
                    Player.Instance.ChangeWield(EquipSlot.RIGHTHAND, interactableObject.GetComponent<IInteractable>());
                    break;
            }
            interactableObject.transform.parent = transform;
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && interactableObject != null)
        {
            Debug.Log("released");
            interactableObject.transform.parent = null;
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
        if(interactableObject == null)
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
