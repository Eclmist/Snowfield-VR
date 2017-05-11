using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerFSM))]
public class AdventurerAI : AI {

    private List<StoryHunt> onGoingQuests = new List<StoryHunt>();

    private List<Relation> actorRelations = new List<Relation>();

    protected override void Awake()
    {
        base.Awake();
        AddJob(JobType.ADVENTURER);
    }

    
    public List<StoryHunt> Quests
    {
        get
        {
            return onGoingQuests;
        }
    }

 

}
