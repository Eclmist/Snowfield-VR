using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem
{
	[SerializeField] protected GameObject fakeSelf;
	[SerializeField] protected PhysicalMaterial.Type materialType = PhysicalMaterial.Type.IRON;
	[SerializeField] protected int weaponTier = 0;

    #region PlayerInteraction
    protected bool removable = true, toggled = false;
    [SerializeField]
    [Tooltip("Intended Pivot of the sword, doesnt need to be child(Calculated on awake)")]
    protected Transform pivot;

    protected Vector3 offsetPosition;
    protected Quaternion offsetRotation;
    protected override void Start()
    {
        base.Start();
        if (!pivot)
        {
            offsetPosition = Vector3.zero;
            offsetRotation = Quaternion.identity;
        }
        else
        {

            offsetPosition = pivot.localPosition;
            offsetRotation = pivot.localRotation;
        }
        
    }
    protected virtual void UseItem()
    {
        Debug.Log("You are using " + this.name);
    }


	public GameObject GetFakeself()
	{
		return fakeSelf;
	}

	public PhysicalMaterial.Type GetPhysicalMaterial()
	{
		return materialType;
	}

	public int WeaponTier
	{
		get { return weaponTier; }
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

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        VR_Interactable_UI UI = GetComponent<VR_Interactable_UI>();
        if (currentInteractingController != null && collision.gameObject != currentInteractingController.gameObject && UI == null)
        {
            PlaySound(currentInteractingController.Velocity.magnitude > maxSwingForce ? 1 : currentInteractingController.Velocity.magnitude / maxSwingForce);
            removable = false;
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
        }
    }

    protected override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
        VR_Interactable_UI UI = GetComponent<VR_Interactable_UI>();
        if (currentInteractingController != null && collision.gameObject != currentInteractingController.gameObject && UI == null)
        {
            removable = true;
        }
    }
    #endregion

}
