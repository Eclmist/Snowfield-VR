using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI {

    private List<StoryHunt> onGoingQuests = new List<StoryHunt>();

    private List<Relation> actorRelations = new List<Relation>();

    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();

    protected override void Awake()
    {
        base.Awake();
        AddJob(JobType.ADVENTURER);
        GetSlotsAndEquipment();
    }

    protected void GetSlotsAndEquipment()
    {
        EquipSlot[] equipmentSlots = GetComponentsInChildren<EquipSlot>();
        foreach (EquipSlot slot in equipmentSlots)
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

        foreach (Equipment equip in inventory)
        {

            ChangeWield(Instantiate(equip));
        }
    }


    public override void ChangeWield(Equipment item)
    {
        switch (item.Slot)
        {
            case EquipSlot.EquipmentSlotType.LEFTHAND:
                if (leftHand.Item != null)
                    leftHand.Item.Unequip();
                leftHand.Item = item;
                leftHand.Item.Equip(leftHand.transform);
                break;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                if (rightHand.Item != null)
                    rightHand.Item.Unequip();
                rightHand.Item = item;
                rightHand.Item.Equip(rightHand.transform);
                break;
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
