using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : MonoBehaviour {

    public enum EquipmentSlotType
    {
        LEFTHAND,
        RIGHTHAND,
    }

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
    private Equipment slotItem;

    public Equipment Item
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

    private void Awake()
    {
        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
            slotItem = weapon;
    }
}
