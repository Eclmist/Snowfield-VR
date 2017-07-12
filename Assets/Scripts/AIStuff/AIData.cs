using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorData {

    protected string prefabPath;

    protected List<Job> jobList = new List<Job>();

    protected int maxHealth;//Can be replaced with generic script

    protected string name;
    
    //put inventory crap here

    public string Path
    {
        get
        {
            return prefabPath;
        }
    }

    public List<Job> JobList
    {
        get
        {
            return jobList;
        }
    }

    public int Health
    {
        get
        {
            return maxHealth;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public ActorData(string _prefabPath,string _name)
    {
        prefabPath = _prefabPath;
        jobList = new List<Job>();
        name = _name;
        maxHealth = 100;//replacable
    }
}

[System.Serializable]
public class AdventurerAIData : ActorData
{
    private QuestBook questBook;

    public AdventurerAIData(string _prefabPath,string _name) : base(_prefabPath,_name)
    {
        questBook = new QuestBook();
        Job newJob = new Job(JobType.ADVENTURER);
        jobList.Add(newJob);
    }

    public QuestBook QuestBook
    {
        get
        {
            return questBook;
        }
    }
}
