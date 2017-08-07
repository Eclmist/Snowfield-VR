using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : FriendlyAI
{
    //private List<Relation> actorRelations = new List<Relation>();

    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();

    

    [SerializeField]
    protected AdventurerAIData data;

    public override ActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = (AdventurerAIData)value;
        }
    }

   
    protected override void Awake()
    {
        base.Awake();
        GetSlots();
    }

    
    protected override void Start()
    {
        base.Start();
        if (QuestBook.StoryQuests == null)
            QuestBook.BeginQuestBook();
    }



    public void UnEquipWeapons()
    {
        if (leftHand != null && leftHand.Item != null)
        {
            if (variable.GetStat(Stats.StatsType.HEALTH).Current > 0)
                Destroy(leftHand.Item.gameObject);
            else
                leftHand.Item.Unequip();
        }
        if (rightHand != null && rightHand.Item != null)
        {
            if (variable.GetStat(Stats.StatsType.HEALTH).Current > 0)
                Destroy(rightHand.Item.gameObject);
            else
                rightHand.Item.Unequip();
        }
    }

    public QuestBook QuestBook
    {
        get
        {
            return data.QuestBook;
        }
    }

    public bool EquipRandomWeapons()
    {
        foreach (Equipment equip in inventory)
        {
            if (equip is Weapon)
            {
                ChangeWield(Instantiate(equip));
                return true;
            }
        }
        return false;
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

   
    //protected System.Collections.IEnumerator StartInteraction(OptionPane op)
    //{
    //    isInteracting = true;

    //    while (true)
    //    {
    //        if (!isInteracting || !op)
    //        {
    //            if (op)
    //                op.ClosePane();
    //            break;
    //        }

    //        yield return new WaitForEndOfFrame();
    //    }

    //    isInteracting = false;

    //}


    public override void GainExperience(JobType jobType, int value)
    {
        Job currentJob = data.GetJob(jobType);
        if (currentJob != null)
        {
            int tempCurrentLevel = currentJob.Level;
            base.GainExperience(jobType, value);
            if (currentJob.Level > tempCurrentLevel && gameObject.activeSelf == true)
                TextSpawnerManager.Instance.SpawnText("Level Up!", Color.green, transform, 4);
        }
    }

    

    public override void Spawn()
    {
        base.Spawn();
        (currentFSM as AdventurerFSM).NewSpawn();
        if (variable)
        {
            variable.ResetCurrentVariables();
            variable.UpdateVariables();
        }
    }

    public override float GetOutOfTimeDuration()
    {
        float totalDuration = 25f;
        QuestEntry<StoryQuest> quest = QuestBook.GetFastestQuest();
        if (quest != null)
            totalDuration += quest.RemainingProgress;
        //Can add more time here when taking into consideration item get
        return totalDuration;
    }

    public override void OutOfTownProgress()//This method is ran by the aimanager every "tick out of town"
    {
        QuestEntry<StoryQuest> quest = QuestBook.GetFastestQuest();
        if (quest != null)
            quest.QuestProgress();
        GainExperience(JobType.COMBAT, 1);
        //Can get misc items here
    }

    public override void TakeDamage(float damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if (variable.GetStat(Stats.StatsType.HEALTH).Current <= 0)
        {
            AIManager.Instance.Spawn(this, data.GetJob(JobType.COMBAT).Level * 20, TownManager.Instance.GetRandomSpawnPoint());
            UnEquipWeapons();
        }
    }


}