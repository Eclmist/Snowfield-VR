using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEntry
{

    private Quest currentQuest;
    private bool isCompleted;

    public QuestEntry(Quest quest)
    {
        isCompleted = false;
        currentQuest = quest;
    }
}

public class QuestEntryGroup<T> where T : Quest
{
    private List<T> quests;

    private bool isCompleted;

    private int progressionIndex;

    private JobType jobType;

    public JobType JobType
    {
        get { return this.jobType; }
    }

    public List<T> Quest
    {
        get
        {
            return quests;
        }
    }

    public int ProgressionIndex
    {
        get
        {
            return progressionIndex;
        }
        set
        {
            progressionIndex = value;
        }
    }

    public bool Completed
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

    public QuestEntryGroup(JobType type)
    {
        isCompleted = false;
        quests = new List<T>();
        progressionIndex = 0;
        jobType = type;
    }

    public int Count
    {
        get { return quests.Count; }
    }

    public void Add(T _item)
    {
        quests.Add(_item);
    }

    public T this[int index]   // Indexer declaration  
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

}
