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

	protected Collider colObject = null;
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
	public override void OnFixedUpdateInteraction (VR_Controller_Custom referenceCheck)
	{

	}
	public int WeaponTier
	{
		get { return weaponTier; }
	}

	protected override void Update()
	{
		base.Update();
		if (colObject == null)
			removable = true;
		else
			removable = false;
	}
	public override void OnTriggerPress(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck != currentInteractingController)
        {
            base.OnTriggerPress(referenceCheck);
            rigidBody.useGravity = false;
            itemCollider.isTrigger = true;
            toggled = true;
			targetPositionPoint.transform.position = referenceCheck.transform.position + transform.rotation * offsetPosition;;
			targetPositionPoint.transform.rotation = referenceCheck.transform.rotation * offsetRotation;
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

        if (currentInteractingController != null && collision.GetComponentInParent<VR_Controller_Custom>() != currentInteractingController)
        {
            PlaySound(currentInteractingController.Velocity.magnitude > maxSwingForce ? 1 : currentInteractingController.Velocity.magnitude / maxSwingForce);
			currentInteractingController.Vibrate (currentInteractingController.Velocity.magnitude > maxSwingForce ? 10 : (currentInteractingController.Velocity.magnitude / maxSwingForce) * 10);

			colObject = collision;
        }
    }

	protected override void OnTriggerStay(Collider collision)
	{
		base.OnTriggerStay(collision);

		if (currentInteractingController != null && collision.GetComponentInParent<VR_Controller_Custom>() != currentInteractingController)
		{
			colObject = collision;
		}
	}

	public override void OnUpdateInteraction(VR_Controller_Custom controller)
    {
        base.OnUpdateInteraction(controller);
        transform.rotation = controller.transform.rotation * offsetRotation;
        transform.position = controller.transform.position + transform.rotation * offsetPosition;

    }

    protected override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);

        if (currentInteractingController != null && collision.GetComponentInParent<VR_Controller_Custom>() != currentInteractingController)
        {
			colObject = null;
		}
    }

	
    #endregion

}
