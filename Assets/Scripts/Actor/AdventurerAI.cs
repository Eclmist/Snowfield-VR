using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI {

    private List<Quest> onGoingQuests = new List<Quest>();

    private List<Relation> actorRelations = new List<Relation>();

    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();

    protected override void Awake()
    {
        base.Awake();
        AddJob(JobType.ADVENTURER);
        EquipSlot[] equipmentSlots = GetComponentsInChildren<EquipSlot>();
        foreach(EquipSlot slot in equipmentSlots)
        {
            switch (slot.CurrentType)
            {
                case EquipSlot.EquipmentSlotType.LEFTHAND:
                    leftHand = slot;
                    break;
                case EquipSlot.EquipmentSlotType.RIGHTHAND:
                    rightHand = slot;
                    break;
            }
        }

        foreach(Equipment equip in inventory)
        {
            switch (equip.Slot)
            {
                case EquipSlot.EquipmentSlotType.LEFTHAND:
                    leftHand.Item = Instantiate(equip, leftHand.transform);
                    break;
                case EquipSlot.EquipmentSlotType.RIGHTHAND:
                    rightHand.Item = Instantiate(equip, rightHand.transform);
                    break;
            }
        }

        
    }

    
    public List<Quest> Quests
    {
        get
        {
            return onGoingQuests;
        }
    }

 

}
