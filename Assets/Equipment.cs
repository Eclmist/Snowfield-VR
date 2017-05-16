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

    public void Equip(Transform parent)
    {
        rigidBody.isKinematic = true;
        itemCollider.enabled = false;
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Unequip()
    {
        rigidBody.isKinematic = false;
        itemCollider.enabled = true;
        transform.parent = null;
    }
}
