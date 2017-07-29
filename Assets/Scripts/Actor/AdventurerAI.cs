using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI
{
    //private List<Relation> actorRelations = new List<Relation>();
    [SerializeField]
    protected AdventurerAIData data;

    [SerializeField]
    private List<Equipment> inventory = new List<Equipment>();

    private List<InteractionsWithPlayer> interactionsWithPlayer = new List<InteractionsWithPlayer>();

    protected override void Awake()
    {
        base.Awake();
        GetSlots();
    }

    public void StopAllInteractions()
    {
        foreach(InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            interaction.StopInteraction();
        }
    }
    protected virtual void Start()
    {
        interactionsWithPlayer.AddRange(GetComponents<InteractionsWithPlayer>());
        if (QuestBook.StoryQuests == null)
            QuestBook.BeginQuestBook();
    }

    public bool Interacting
    {
        get
        {
            foreach(InteractionsWithPlayer interaction in interactionsWithPlayer)
            {
                if (interaction.IsInteracting)
                    return true;
            }
            return false;
        }
    }
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

    public bool IsInteractionAvailable()
    {
        foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            if (!interaction.Interacted)
                return true;
        }

        return false;
    }

    public bool StartInteraction()
    {
        foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            if (!interaction.Interacted)
            {
                interaction.StartInteraction();
                return true;
            }
        }

        return false;
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

    public int Level
    {
        get
        {
            return data.CurrentJob.Level;
        }
    }

    public void GainExperience(int value)
    {
        int tempCurrentLevel = data.CurrentJob.Level;
        data.CurrentJob.GainExperience(value);
        if (data.CurrentJob.Level > tempCurrentLevel)
            TextSpawnerManager.Instance.SpawnText("Level Up!",Color.green,transform,4);
        
    }

    public override void Interact(Actor actor)
    {
		(currentFSM as AdventurerFSM).StartInteractRoutine (actor);
    }

    public override void Spawn()
    {
        base.Spawn();
		(currentFSM as AdventurerFSM).NewSpawn ();
		if(variable)
		variable.ResetHealth ();
    }

    public override float GetOutOfTimeDuration()
    {
        float totalDuration = 25f;
        QuestEntry<StoryQuest> quest = data.QuestBook.GetFastestQuest();
        if (quest != null)
            totalDuration += quest.RemainingProgress;
        //Can add more time here when taking into consideration item get
        return totalDuration;
    }

    public override void OutOfTownProgress()//This method is ran by the aimanager every "tick out of town"
    {
        QuestEntry<StoryQuest> quest = data.QuestBook.GetFastestQuest();
        if (quest != null)
            quest.QuestProgress();
        GainExperience(1);
        //Can get misc items here
    }

    public override void TakeDamage(int damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if (Health <= 0)
            AIManager.Instance.Spawn(this, data.CurrentJob.Level * 20, TownManager.Instance.GetRandomSpawnPoint());
    }


}