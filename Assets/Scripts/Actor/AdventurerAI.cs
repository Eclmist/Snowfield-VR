using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI {

    private List<Quest> onGoingQuests = new List<Quest>();

    private List<Relation> actorRelations = new List<Relation>();

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
    }

    
    public List<Quest> Quests
    {
        get
        {
            return onGoingQuests;
        }
    }

 

}
