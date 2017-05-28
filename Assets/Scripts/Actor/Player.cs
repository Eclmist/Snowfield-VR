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

    [SerializeField]
    private Transform interactableArea;

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
        if (Vector3.Distance(interactableArea.position, transform.position) < 2)
        {

            if (actor is AdventurerAI)
            {
                (actor as AdventurerAI).IsConversing = true;
                foreach (QuestEntryGroup<StoryQuest> group in (actor as AdventurerAI).QuestBook.StoryQuests)
                {
                    QuestEntry<StoryQuest> quest = (actor as AdventurerAI).QuestBook.GetCompletableQuest(group);
                    if (quest != null)
                    {
                        DialogManager.Instance.AddDialog<QuestEntry<StoryQuest>>((actor as AdventurerAI).EndQuest, quest);
                    }
                }

                foreach (QuestEntryGroup<StoryQuest> group in (actor as AdventurerAI).QuestBook.StoryQuests)
                {
                    QuestEntry<StoryQuest> quest = (actor as AdventurerAI).QuestBook.GetStartableQuest(group);
                    if (quest != null)
                    {
                        DialogManager.Instance.AddDialog<QuestEntry<StoryQuest>>((actor as AdventurerAI).StartQuest, quest);
                    }
                }
                //if(!HuntManager.Instance.GetNextQuest(story))
                // (adventurerAI)actor.DoneConversing();
                //Do further checks for buy selling etc
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(interactableArea.position, 1);
    //}

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