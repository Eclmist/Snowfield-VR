using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : CraftedItem, IBlock {

    [SerializeField]
    private EquipSlot.EquipmentSlotType slot;

    private Actor owner;

    public Actor Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }


    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {

        if (referenceCheck != linkedController)
        {

            owner = Player.Instance;
        }

        base.StartInteraction(referenceCheck);

    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (removable && !toggled)
        {
            owner = null;
            base.StopInteraction(referenceCheck);
        }
    }

   
    
    public EquipSlot.EquipmentSlotType Slot
    {
        get
        {
            return slot;
        }
    }

    public void Equip(Transform parent)
    {
        rigidBody.isKinematic = true;
        itemCollider.isTrigger = true;
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Unequip()
    {
        rigidBody.isKinematic = false;
        itemCollider.isTrigger = false;
        transform.parent = null;
    }

    

    

}
