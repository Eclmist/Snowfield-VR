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
    private Rigidbody attachedPoint;
    private SteamVR_TrackedObject trackedObject;
    private GameObject interactableObject;
    private GameObject interactedObject;

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        attachedPoint = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControllerInput();
    }

    private void ControllerInput()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
        if (interactedObject == null && interactableObject != null && (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)))
        {
            IInteractable interactable = interactableObject.GetComponent<IInteractable>();
            interactable.Interact(transform, attachedPoint);


            switch (handle)
            {
                case Controller_Handle.LEFT:
                    Player.Instance.ChangeWield(EquipSlot.LEFTHAND, interactable);
                    break;
                case Controller_Handle.RIGHT:
                    Player.Instance.ChangeWield(EquipSlot.RIGHTHAND, interactable);
                    break;
            }



        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            IInteractable interactable = interactedObject.GetComponent<IInteractable>();
            Rigidbody rigidBody = interactedObject.GetComponent<Rigidbody>();

            if (interactable != null)
            {
                interactable.StopInteraction(transform);
            }

            Transform origin = trackedObject.origin ? trackedObject.origin : trackedObject.transform.parent;
            if (origin != null)
            {
                rigidBody.velocity = origin.TransformVector(device.velocity);
                rigidBody.angularVelocity = origin.TransformVector(device.angularVelocity);
            }
            else
            {
                rigidBody.velocity = device.velocity;
                rigidBody.angularVelocity = device.angularVelocity;
            }

            rigidBody.maxAngularVelocity = rigidBody.angularVelocity.magnitude;
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
            Debug.Log("ExitTriggerable");
            interactableObject = null;
        }
        else
        {
            Debug.Log("Exited");
        }
    }
}
