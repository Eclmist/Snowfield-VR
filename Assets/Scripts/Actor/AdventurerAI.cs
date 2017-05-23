using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI {

    

    private List<Relation> actorRelations = new List<Relation>();

    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();
    private QuestBook questBook;
    protected override void Awake()
    {
        base.Awake();
        AddJob(JobType.ADVENTURER);
        GetSlots();
    }

    protected virtual void Start()
    {
        questBook = new QuestBook();
    }

    public QuestBook QuestBook
    {
        get
        {
            return questBook;
        }
    }
    public void EquipRandomWeapons()
    {
        foreach (Equipment equip in inventory)
        {
            if (equip.Slot == EquipSlot.EquipmentSlotType.LEFTHAND || equip.Slot == EquipSlot.EquipmentSlotType.RIGHTHAND)
                ChangeWield(Instantiate(equip));
        }
    }

    public void UnEquipWeapons()
    {
        if (leftHand.Item != null)
            leftHand.Item.Unequip();
        if (rightHand.Item != null)
            rightHand.Item.Unequip();
    }

    protected void GetSlots()
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
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        TakeDamage(1, Player.Instance);
    //    }
    //}
    public override void TakeDamage(int damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        
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
   
    public bool GotLobang()
    {
        foreach(QuestEntryGroup<StoryQuest> group in questBook.StoryQuests)
        {
            if (questBook.GetCompletableQuest(group) != null || questBook.GetStartableQuest(group) != null)
                return true;
        }
        return false;
    }

    public override void DoneConversing()
    {
        currentFSM.ChangeState(ActorFSM.FSMState.IDLE);
        isConversing = false;
        Debug.Log(isConversing);
    }

    public void StartQuest(QuestEntry<StoryQuest> hunt)
    {
        StartCoroutine(hunt.StartQuest(10));
        Debug.Log(hunt.Started);
        DoneConversing();
    }

    public void EndQuest(QuestEntry<StoryQuest> hunt)
    {
        Debug.Log("fdfsdfsdfs");
    }

}
