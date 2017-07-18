using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorData
{

    protected string prefabPath;

    protected string name;

    [SerializeField]
    protected Stats baseStats;

    public Stats BaseStats
    {
        get
        {
            return baseStats;
        }
    }

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

    public ActorData(Stats _baseStats, string _name, string _prefabPath = "")
    {
        baseStats = _baseStats;
        prefabPath = _prefabPath;
        name = _name;
    }

    public virtual int Attack
    {
        get
        {
            return baseStats.Attack;
        }
    }

    public virtual int Health
    {
        get
        {
            return baseStats.Health;
        }
    }

    public virtual int HealthRegeneration
    {
        get
        {
            return baseStats.HealthRegeneration;
        }
    }

    public virtual int MovementSpeed
    {
        get
        {
            return baseStats.MovementSpeed;
        }
    }
}

[System.Serializable]
public class CombatActorData : ActorData
{
    [SerializeField]
    protected CombatJob combatJob;
    public CombatActorData(CombatJob _combatJob, Stats _stats, string _name, string _prefabPath = "") : base(_stats, _name, _prefabPath)
    {
        combatJob = new CombatJob(JobType.COMBAT, _combatJob);
    }

    public CombatJob CurrentJob
    {
        get
        {
            return combatJob;
        }
    }

    public override int Attack
    {
        get
        {
            return base.Attack + combatJob.DPL * combatJob.Level;
        }
    }

    public override int Health
    {
        get
        {
            return base.Health + combatJob.HPL * combatJob.Level;
        }
    }

    public override int HealthRegeneration
    {
        get
        {
            return base.HealthRegeneration + combatJob.HRPL * combatJob.Level;
        }
    }
}

[System.Serializable]
public class AdventurerAIData : CombatActorData
{

    private QuestBook questBook;

    public AdventurerAIData(CombatJob _combatJob, Stats _stats, string _name, string _prefabPath = "") : base(_combatJob, _stats, _name, _prefabPath)
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

    public PlayerData(CombatJob _combatJob, Stats _stats, string _name, string _prefabPath = "") : base(_combatJob, _stats, _name, _prefabPath)
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