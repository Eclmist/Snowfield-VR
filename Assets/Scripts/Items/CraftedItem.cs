using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem
{
    #region PlayerInteraction
    protected bool removable = true, toggled = false;
    [SerializeField]
    [Tooltip("Intended Pivot of the sword, doesnt need to be child(Calculated on awake)")]
    protected Transform pivot;

    protected Vector3 offsetPosition;
    protected Quaternion offsetRotation;
    protected override void Awake()
    {
        base.Awake();
        if (!pivot)
            pivot = transform;
        offsetPosition = transform.position - pivot.position;
        offsetRotation = pivot.rotation;

    }
    protected virtual void UseItem()
    {
        Debug.Log("You are using " + this.name);
    }



    public override void OnTriggerPress(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck != currentInteractingController)
        {
            base.OnTriggerPress(referenceCheck);
            rigidBody.useGravity = false;
            itemCollider.isTrigger = true;
            toggled = true;
        }
        else
        {
            toggled = false;
        }
        //rigidBody.maxAngularVelocity = 100f;
    }

    public override void OnTriggerRelease(VR_Controller_Custom referenceCheck)
    {

        if (removable && !toggled)
        {
            base.OnTriggerRelease(referenceCheck);
            itemCollider.isTrigger = false;
            rigidBody.useGravity = true;
        }
    }



    //public override void UpdatePosition()
    //{
    //    transform.position = linkedController.transform.position;
    //    transform.rotation = linkedController.transform.rotation;
    //}

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (currentInteractingController != null)
        {
            PlaySound(currentInteractingController.Velocity.magnitude > maxSwingForce ? 1 : currentInteractingController.Velocity.magnitude / maxSwingForce);

        }
    }

    public override void OnInteracting(VR_Controller_Custom controller)
    {
        base.OnInteracting(controller);
        transform.rotation = controller.transform.rotation * offsetRotation;
        transform.position = controller.transform.position + transform.rotation * offsetPosition;

    }

    protected virtual void OnTriggerStay(Collider collision)
    {
        if (currentInteractingController != null && collision.gameObject != currentInteractingController.gameObject)
        {
            removable = false;
            currentInteractingController.Vibrate(1);
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if (currentInteractingController != null && collision.gameObject != currentInteractingController.gameObject)
        {
            removable = true;
        }
    }
    #endregion

}
