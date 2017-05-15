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

    public void Equip()
    {
        rigidBody.isKinematic = true;
    }
}
