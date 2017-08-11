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



    public void DestroyHandsWeapon()
    {
        if (leftHand != null && leftHand.Item != null)
        {
            Destroy(leftHand.Item.gameObject);
            leftHand.Item = null;
        }
        if (rightHand != null && rightHand.Item != null)
        {
            Destroy(rightHand.Item.gameObject);
            rightHand.Item = null;
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
        float currentDamage = -1;
        Weapon currentWeapon = null;
        foreach (Equipment equip in inventory)
        {
            if (equip is Weapon)
            {
                if ((equip as Weapon).Damage > currentDamage)
                {
                    currentDamage = (equip as Weapon).Damage;
                    currentWeapon = (equip as Weapon);
                }

            }
        }
        if (currentWeapon != null)
        {
            ChangeWield(Instantiate(currentWeapon));
            return true;
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
        if (statsContainer)
        {
            statsContainer.ResetCurrentVariables();
            statsContainer.UpdateVariables();
        }
    }

    public virtual float GetOutOfTimeDuration()
    {
        float totalDuration = 25f;
        QuestEntry<StoryQuest> quest = QuestBook.GetFastestQuest();
        if (quest != null)
            totalDuration += quest.RemainingProgress;
        //Can add more time here when taking into consideration item get
        return totalDuration;
    }

    public virtual void OutOfTownProgress()//This method is ran by the aimanager every "tick out of town"
    {
        QuestEntry<StoryQuest> quest = QuestBook.GetFastestQuest();
        if (quest != null)
            quest.QuestProgress();
        GainExperience(JobType.COMBAT, 1);
        //Can get misc items here
    }

    public override void TakeDamage(float damage, Actor attacker, JobType type)
    {
        base.TakeDamage(damage, attacker,type);
        if (statsContainer.GetStat(Stats.StatsType.HEALTH).Current <= 0)
        {
            AIManager.Instance.Respawn(this);
        }
    }

    protected override void DropItem()
    {
        base.DropItem();
        if (leftHand != null && leftHand.Item != null)
        {
            leftHand.Item.Unequip();
            leftHand.Item.gameObject.AddComponent<DroppedItem>();
            leftHand.Item = null;
        }
        if (rightHand != null && rightHand.Item != null)
        {
            rightHand.Item.Unequip();
            rightHand.Item.gameObject.AddComponent<DroppedItem>();
            rightHand.Item = null;
        }
    }

    public override void DealDamage(float damage, IDamagable target,JobType damageType)
    {
        base.DealDamage(damage, target,damageType);
        CombatManager.Instance.PlayRandomHitSoundAt(target.transform);

    }

    public override void Attack(IDamage item, IDamagable target, float scale = 1)
    {
        base.Attack(item, target, scale);
        CombatManager.Instance.PlayRandomHitSoundAt(target.transform);
    }



}