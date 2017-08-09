using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : CraftedItem {

    [SerializeField]
    private EquipSlot.EquipmentSlotType slot;

    public EquipSlot.EquipmentSlotType Slot
    {
        get
        {
            return slot;
        }
    }

    public virtual void Equip(Transform parent)
    {
        rigidBody.isKinematic = true;
        itemCollider.isTrigger = true;
        transform.parent = parent;
        transform.rotation = parent.rotation * pivot.localRotation;
        transform.position = parent.position + transform.rotation * -pivot.localPosition;
    }

    public virtual void Unequip()
    {
        rigidBody.isKinematic = false;
        transform.parent = null;
        itemCollider.isTrigger = false;
    }

    

    

}
