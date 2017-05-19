using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : CraftedItem, IBlock {

    [SerializeField]
    private EquipSlot.EquipmentSlotType slot;
    [SerializeField]
    protected float range;

    private bool canBlock;

    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {

        if (referenceCheck != linkedController)
        {
            
            Equip(Player.Instance.transform);
        }

        base.StartInteraction(referenceCheck);

    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (removable && !toggled)
        {
            base.StopInteraction(referenceCheck);
            Unequip();
        }
    }

    public bool CanBlock
    {
        get
        {
            return canBlock;
        }
        set
        {
            canBlock = value;
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
        canBlock = true;
    }

    public void Unequip()
    {
        rigidBody.isKinematic = false;
        itemCollider.isTrigger = false;
        transform.parent = null;
        canBlock = false;
    }

    

    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

}
