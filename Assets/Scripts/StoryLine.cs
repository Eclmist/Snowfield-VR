using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryLine {

    [SerializeField]
    private JobType jobType;
    [SerializeField]
    private List<StoryQuest> quests;
    [SerializeField]
    private bool isCompleted;

    public StoryLine(JobType jobType)
    {
        this.jobType = jobType;
        quests = new List<StoryQuest>();
        quests.Add(new StoryQuest(jobType));
    }

    

    public  bool Completed
    {
        get
        {
            return isCompleted;
        }
        set
        {
            isCompleted = value;
        }
    }

    public JobType JobType
    {
        get { return this.jobType; }
    }

    public List<StoryQuest> Quests
    {
        get { return this.quests; }
        set { this.quests = value; }
    }

 
    public int Count
    {
        get { return quests.Count; }
    }

    public void Add(StoryQuest _item)
    {
        quests.Add(_item);
    }

    public StoryQuest this[int index]   // Indexer declaration  
    {
        get
        {
            return quests[index];
        }
        set
        {
            quests[index] = value;
        }
    }
   
   
    public bool Contains(StoryQuest _item)
    {
        return (quests.Contains(_item));
    }

    //public StoryHunt GetNextHunt(StoryHunt hunt)
    //{
    //    return quests[hunt.ProgressionIndex + 1];
    //}

    public void Clear()
    {
        quests.Clear();
    }


}
