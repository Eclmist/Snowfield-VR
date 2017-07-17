using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorData
{

    protected string prefabPath;

    protected string name;

    //put inventory crap here

    public string Path
    {
        get
        {
            return prefabPath;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public ActorData(string _name, string _prefabPath = "")
    {
        prefabPath = _prefabPath;
        name = _name;
    }
}

[System.Serializable]
public class CombatActorData : ActorData
{
    [SerializeField]
    protected CombatJob combatJob;
    public CombatActorData(string _name, int _DPL, int _HPL, int _HRPL, int movementSpeed, string _prefabPath = "") : base(_name, _prefabPath)
    {
        combatJob = new CombatJob(JobType.COMBAT, _DPL, _HPL, _HRPL,movementSpeed);
        Debug.Log(combatJob.HRPL);
    }

    public CombatJob CurrentJob
    {
        get
        {
            return combatJob;
        }
    }
}

[System.Serializable]
public class AdventurerAIData : CombatActorData
{

    private QuestBook questBook;

    public AdventurerAIData(string _name, int _DPL, int _HPL, int _HRPL,int movementSpeed, string _prefabPath = "") : base(_name, _DPL, _HPL, _HRPL,movementSpeed, _prefabPath)
    {
        questBook = new QuestBook();
    }

    public QuestBook QuestBook
    {
        get
        {
            return questBook;
        }
    }
}

[System.Serializable]
public class PlayerData : CombatActorData
{
    [SerializeField]
    protected List<Job> jobList = new List<Job>();
    protected int gold;
    public PlayerData(string _name, int _DPL, int _HPL, int _HRPL, string _prefabPath = "") : base(_name, _DPL, _HPL, _HRPL, 0, _prefabPath)
    {
        gold = 0;
    }

    public List<Job> JobList
    {
        get
        {
            return jobList;
        }
    }

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
        }
    }

}