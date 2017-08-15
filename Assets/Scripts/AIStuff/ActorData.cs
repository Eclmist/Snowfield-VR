using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorData
{

    protected string prefabPath;

    protected string name;

    [SerializeField]
    protected List<Job> jobList = new List<Job>();

    public List<Job> ListOfJobs
    {
        get
        {
            return jobList;
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

    public ActorData(ActorData copyData, string _name, string _prefabPath = "")
    {
        prefabPath = _prefabPath;
        name = _name;

        foreach(Job job in copyData.jobList)
        {
            Job newJob = new Job(job.Type);
            foreach(Stats stat in job.BonusStats)
            {
                Stats s = new Stats(stat);
                newJob.AddStats(s);
            }
            jobList.Add(newJob);
        }
    }

    public Job GetJob(JobType j)
    {
        foreach(Job job in ListOfJobs)
        {
            if (job.Type == j)
                return job;
        }
        return null;
    }

    public Job AddJob(JobType newJobType)
    {
        Job newJob = new Job(newJobType);
        ListOfJobs.Add(newJob);
        return newJob;
    }

}


[System.Serializable]
public class AdventurerAIData : ActorData
{
    [System.Serializable]
    public class CharacterInformation
    {

        protected float scale;

        protected float r, g, b;
        

        public CharacterInformation(float _scale, Color _hairColor)
        {
            scale = _scale;

            r = _hairColor.r;
            g = _hairColor.g;
            b = _hairColor.b;
        }

        public float Scale
        {
            get
            {
                return scale;
            }
        }

        public Color HairColor
        {
            get
            {
                return new Color(r, g, b);
            }
        }
    }
    private QuestBook questBook;

    private CharacterInformation characterCustomizeInfo;

    public CharacterInformation CustomizeInfo
    {
        get
        {
            return characterCustomizeInfo;
        }
    }

    [SerializeField]
    protected Inventory inventory;

    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
    }

    public AdventurerAIData(CharacterInformation _ci, ActorData data,string _name, string _prefabPath = "") : base(data, _name, _prefabPath)
    {
        questBook = new QuestBook();
        characterCustomizeInfo = _ci;
        inventory = new Inventory();
        inventory.AddToInventory(WeaponTierManager.Instance.GetWeapon(PhysicalMaterial.Type.BRONZE, 1));
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
public class MonsterData : ActorData
{
    [SerializeField]
    protected int monsterTier;

    public int Tier
    {
        get
        {
            return monsterTier;
        }
    }

    public MonsterData(ActorData data, string _name, string _prefabPath = "") : base(data, _name, _prefabPath)
    {
        
    }

}

[System.Serializable]
public class PlayerData : ActorData
{ 
    [SerializeField]
    protected int gold, expCrates,currentTax;

    public PlayerData(ActorData data, string _name, string _prefabPath = "") : base(data, _name, _prefabPath)
    {
        gold = 9000;
        expCrates = 0;
        currentTax = 0;
    }

    public int Tax
    {
        get
        {
            return currentTax;
        }
        set
        {
            currentTax = value;
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

    public int EXPCrates
    {
        get
        {
            return expCrates;
        }
        set
        {
            expCrates = value;
        }
    }

}