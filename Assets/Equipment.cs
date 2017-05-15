using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : CraftedItem {

    private EquipSlot.EquipmentSlotType slot;

    public EquipSlot.EquipmentSlotType Slot
    {
        get
        {
            return slot;
        }
    }

}
