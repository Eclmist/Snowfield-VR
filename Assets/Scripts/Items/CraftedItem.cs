using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedItem : GenericItem
{
    #region PlayerInteraction
    protected bool removable = true, toggled = false;
    protected virtual void UseItem()
    {
        Debug.Log("You are using " + this.name);    
    }



    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck != linkedController)
        {
            base.StartInteraction(referenceCheck);
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

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (removable && !toggled)
        {
            itemCollider.isTrigger = false;
            base.StopInteraction(referenceCheck);
            rigidBody.useGravity = true;
            rigidBody.velocity = referenceCheck.Velocity();
            rigidBody.angularVelocity = referenceCheck.AngularVelocity();
        }
    }


    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        if (removable)
        base.Interact(referenceCheck);
        if (toggled)
        {
            transform.position = linkedController.transform.position;
            transform.rotation = linkedController.transform.rotation;
        }
    }
    //public override void UpdatePosition()
    //{
    //    transform.position = linkedController.transform.position;
    //    transform.rotation = linkedController.transform.rotation;
    //}

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if(linkedController != null)
        {
            PlaySound(linkedController.Velocity().magnitude > maxForceVolume ? 1 : linkedController.Velocity().magnitude / maxForceVolume);
            IDamagable target = collision.GetComponent<IDamagable>();
            if (target != null)
                Player.Instance.Attack(this, target);
        }
    }
    protected virtual void OnTriggerStay(Collider collision)
    {
        if (linkedController != null && collision.gameObject != linkedController.gameObject)
        {
            removable = false;
            linkedController.Vibrate(1);
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if (linkedController != null && collision.gameObject != linkedController.gameObject)
        {
            removable = true;
        }
    }
    #endregion
    
}
