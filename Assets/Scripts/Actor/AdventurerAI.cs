﻿
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI
{
    //private List<Relation> actorRelations = new List<Relation>();
    
    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();
    

    protected override void Awake()
    {
        base.Awake();
        
        GetSlots();
    }


    public QuestBook QuestBook
    {
        get
        {
            return (actorData as AdventurerAIData).QuestBook;
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

    //public override void Interact(Actor actor)
    //{
    //    if (actor is Player)
    //        isConversing = true;
    //}
    //public void Interact()
    //{
    //    //IsConversing = true;

    //    //foreach (QuestEntryGroup<StoryQuest> group in questBook.StoryQuests)
    //    //{
    //    //    QuestEntry<StoryQuest> quest = questBook.GetCompletableQuest(group);
    //    //    if (quest != null)
    //    //    {
    //    //        Debug.Log("hit");
    //    //        UIManager.Instance.Instantiate(UIType.OP_YES_NO, quest.Quest.Name, quest.Quest.Dialog.Title, transform.position, Player.Instance.gameObject);
    //    //    }
    //    //}


    //}

    public override void ChangeWield(Equipment item)
    {
        switch (item.Slot)
        {
            case EquipSlot.EquipmentSlotType.LEFTHAND:
                if (leftHand.Item != null)
                    Destroy(leftHand.Item);
                leftHand.Item = item;
                leftHand.Item.Equip(leftHand.transform);

                break;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                if (rightHand.Item != null)
                    Destroy(rightHand.Item);
                rightHand.Item = item;
                rightHand.Item.Equip(rightHand.transform);
                break;
        }
    }

    public bool GotLobang()
    {

        if ((actorData as AdventurerAIData).QuestBook.GetCompletableGroup() != null || (actorData as AdventurerAIData).QuestBook.GetStartableGroup() != null)
            return true;

        return false;
    }

    public void GetLobang()
    {

        
            QuestEntryGroup<StoryQuest> completableGroup = (actorData as AdventurerAIData).QuestBook.GetCompletableGroup();
            if (completableGroup != null)
            {
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_OK, "Quest", "Complete Quest: " + QuestManager.Instance.GetQuest(completableGroup), transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Ok, CompleteQuestDelegate);
                StartCoroutine(StartInteraction(op));
                return;
            }

            QuestEntryGroup<StoryQuest> startableQuest = (actorData as AdventurerAIData).QuestBook.GetStartableGroup();

            if (startableQuest != null)
            {
                OptionPane op = UIManager.Instance.Instantiate(UIType.OP_YES_NO, "Quest", "Start Quest: " + QuestManager.Instance.GetQuest(startableQuest).Name, transform.position, Player.Instance.transform, transform);
                op.SetEvent(OptionPane.ButtonType.Yes, StartQuestYESDelegate);
                op.SetEvent(OptionPane.ButtonType.No, StartQuestNODelegate);
                StartCoroutine(StartInteraction(op));
                return;
            }
        
    }

    protected System.Collections.IEnumerator StartInteraction(OptionPane op)
    {
        isInteracting = true;

        while (true)
        {
            if (!isInteracting || !op)
            {
                if (op)
                    op.ClosePane();
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        isInteracting = false;
      
    }

    public void StartQuestYESDelegate()
    {
        QuestEntryGroup<StoryQuest> startableGroup = (actorData as AdventurerAIData).QuestBook.GetStartableGroup();
        StoryQuest quest = QuestManager.Instance.GetQuest(startableGroup);
        startableGroup.Quest.StartQuest((int)(((quest.RequiredLevel + 1) / (float)GetJob(JobType.ADVENTURER).Level)));
    }

    public void StartQuestNODelegate()
    {
        QuestEntryGroup<StoryQuest> startableGroup = (actorData as AdventurerAIData).QuestBook.GetStartableGroup();
        startableGroup.Quest.Checked = true;
    }

    public void CompleteQuestDelegate()
    {
        QuestEntryGroup<StoryQuest> completableGroup = (actorData as AdventurerAIData).QuestBook.GetCompletableGroup();
        StoryQuest quest = QuestManager.Instance.GetQuest(completableGroup);
        GetJob(JobType.ADVENTURER).GainExperience(quest.Experience);
        QuestBook.RequestNextQuest(completableGroup);
    }


    public bool QuestProgress()
    {
        return (actorData as AdventurerAIData).QuestBook.QuestProgess();
    }

    public override void Interact(Actor actor)
    {
        
        StartCoroutine((currentFSM as AdventurerFSM).Interact(actor));
    }

    public void ResetQuestOnNewTownVisit()
    {
        (actorData as AdventurerAIData).QuestBook.ResetChecked();
    }

}
