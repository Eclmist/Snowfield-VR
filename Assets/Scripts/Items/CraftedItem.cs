using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem
{
	[SerializeField]
	protected GameObject fakeSelf;
	[SerializeField]
	protected PhysicalMaterial.Type materialType = PhysicalMaterial.Type.IRON;
	[SerializeField]
	protected int weaponTier = 0;

	#region PlayerInteraction
	protected bool removable = true, toggled = false;
	[SerializeField]
	[Tooltip("Intended Pivot of the sword")]
	protected Transform pivot;

	protected Collider colObject = null;

	protected Transform Pivot
	{
		get
		{
			if (!pivot)
				return transform;
			else
				return pivot;
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
	public override void OnFixedUpdateInteraction(VR_Controller_Custom referenceCheck)
	{

	}
	public int WeaponTier
	{
		get { return weaponTier; }
	}

	protected override void Update()
	{
		base.Update();
		if (colObject == null || !colObject.enabled)
		{
			removable = true;
			colObject = null;
		}
		else
		{
			Debug.Log(colObject);
			removable = false;
		}
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

		if (currentInteractingController != null && currentInteractingController != collision.GetComponentInParent<VR_Controller_Custom>() && !collision.isTrigger)
		{
			currentInteractingController.Vibrate(currentInteractingController.Velocity.magnitude > maxSwingForce ? 10 : (currentInteractingController.Velocity.magnitude / maxSwingForce) * 10);

			colObject = collision;
		}
	}

	protected override void OnTriggerStay(Collider collision)
	{
		base.OnTriggerStay(collision);

		if (currentInteractingController != null && currentInteractingController != collision.GetComponentInParent<VR_Controller_Custom>() && !collision.isTrigger)
		{
			colObject = collision;
		}
	}

	public override void OnUpdateInteraction(VR_Controller_Custom controller)
	{
		base.OnUpdateInteraction(controller);
		transform.rotation = controller.transform.rotation * Pivot.localRotation;
		transform.position = controller.transform.position + transform.rotation * -Pivot.localPosition;


	}

	protected override void OnTriggerExit(Collider collision)
	{
		base.OnTriggerExit(collision);

		if (currentInteractingController != null && currentInteractingController != collision.GetComponentInParent<VR_Controller_Custom>() && !collision.isTrigger)
		{
			colObject = null;
		}
	}


	#endregion

}
