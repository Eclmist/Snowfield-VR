using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private int gold;

    [SerializeField]
    private Transform vivePosition;

    public static Player Instance;

    public int Gold
    {
        get
        {
            return gold;
        }
    }



    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            AddJob(JobType.BLACKSMITH);
        }
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
    }


    public bool AddGold(int value)
    {
        gold += value;
        return gold >= 0;
    }

    public override void Interact(Actor actor)
    {
        //Message.Instance.IncomingRequest = true;
        
        if (actor is AdventurerAI)
        {
            foreach(QuestEntryGroup<StoryQuest> group in (actor as AdventurerAI).QuestBook.StoryQuests)
            {
                StoryQuest quest = (actor as AdventurerAI).QuestBook.GetCompletableQuest(group);
                if(quest != null)
                    DialogManager.Instance.AddDialog<StoryQuest>((actor as AdventurerAI).QuestBook.EndStoryQuest, quest);
            }

            foreach (QuestEntryGroup<StoryQuest> group in (actor as AdventurerAI).QuestBook.StoryQuests)
            {
                StoryQuest quest = (actor as AdventurerAI).QuestBook.GetStartableQuest(group);
                if (quest != null)
                    DialogManager.Instance.AddDialog<StoryQuest>((actor as AdventurerAI).StartQuest, quest);
            }
            //if(!HuntManager.Instance.GetNextQuest(story))
            // (adventurerAI)actor.DoneConversing();
            //Do further checks for buy selling etc

        }
    }

    public override Transform transform
    {
        get
        {
            return vivePosition;
        }
        set
        {
            vivePosition = value;
        }
    }
}