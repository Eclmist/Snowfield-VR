using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum JobType
{
    BLACKSMITH,
    ALCHEMY,
    COMBAT
}

[System.Serializable]
public class Job {

    [SerializeField]
    protected JobType currentJobType;
    protected int level;
    protected int maxExperience, currentExperience;
    [SerializeField]
    protected List<Stats> additionalStatsPerLevel = new List<Stats>();
    
    public JobType Type
    {
        get
        {
            return currentJobType;
        }
    }

    public int Experience
    {
        get
        {
            return currentExperience;
        }
    }

    public int Level
    {
        get
        {
            return this.level;
        }
    }

    public Job(JobType _currentJob)
    {
        currentJobType = _currentJob;
        level = 1;

        maxExperience = (int)Mathf.Pow(2/GameConstants.Instance.ExpConstant,2);
        currentExperience = 0;
    }

    public void AddStats(Stats s)
    {
        additionalStatsPerLevel.Add(s);
    }

    public void GainExperience(int experienceValue)
    {
        currentExperience += experienceValue;
        if(currentExperience >= maxExperience)
        {
            level++;
            maxExperience = (int)Mathf.Pow(level / GameConstants.Instance.ExpConstant, 2);
        }
    }

    public void SetLevel(int _level)
    {
        level = _level;
        currentExperience = (int)Mathf.Pow((level - 1) / GameConstants.Instance.ExpConstant, 2);
        maxExperience = (int)Mathf.Pow(level / GameConstants.Instance.ExpConstant, 2);
    }

    public List<Stats> BonusStats
    {
        get
        {
            return additionalStatsPerLevel;
        }
    }
}

