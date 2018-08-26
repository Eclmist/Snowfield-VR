using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : MonoBehaviour {

    public enum EquipmentSlotType
    {
        LEFTHAND,
        RIGHTHAND,
    }

    [SerializeField]
    private EquipmentSlotType slotType;

    public EquipmentSlotType CurrentType
    {
        get
        {
            return slotType;
        }
        set
        {
            slotType = value;
        }
    }

    [SerializeField]
    private GenericItem slotItem;

    public GenericItem Item
    {
        get
        {
            return slotItem;
        }
        set
        {
            slotItem = value;
        }
    }
}
