using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem
{

    protected bool removable = true,toggled = false;
    protected virtual void UseItem()
    {
        Debug.Log("You are using " + this.name);
    }



    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        if(removable)
        toggled = !toggled;
        
        if (toggled)
        {
            
            base.Interact(referenceCheck);
            itemCollider.isTrigger = true;
        }
        //rigidBody.maxAngularVelocity = 100f;
    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck && removable && !toggled)
        {
            itemCollider.isTrigger = false;
            base.StopInteraction(referenceCheck);
            rigidBody.velocity = referenceCheck.Velocity();
            rigidBody.angularVelocity = referenceCheck.AngularVelocity();
        }
    }



    public override void UpdatePosition()
    {
        transform.position = linkedController.transform.position;
        transform.rotation = linkedController.transform.rotation;

    }

  
    protected virtual void OnTriggerStay(Collider collision)
    {
        if (linkedController != null && collision.gameObject != linkedController.gameObject)
        {
            removable = false;
            linkedController.Vibrate(5);
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if (linkedController != null && collision.gameObject != linkedController.gameObject)
        {
            removable = true;
        }
    }
}
