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
public class CombatJob : Job
{
    [SerializeField]
    protected int damagePerLevel, healthPerLevel, healthRegenPerLevel, movementSpeed;

    public int DPL
    {
        get
        {
            return damagePerLevel;
        }
    }

    public int HPL
    {
        get
        {
            return healthPerLevel;
        }
    }

    public int HRPL
    {
        get
        {
            return healthRegenPerLevel;
        }
    }

    public CombatJob(JobType currentJob, int _DPL, int _HPL, int _HRPL, int _movementSpeed) : base(currentJob)
    {
        currentJob = JobType.COMBAT;
        damagePerLevel = _DPL;
        healthPerLevel = _HPL;
        healthRegenPerLevel = _HRPL;
        movementSpeed = _movementSpeed;
    }

    public int MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }
}
[System.Serializable]
public class Job {

    protected JobType currentJobType;
    [SerializeField]
    protected int level;
    protected int maxExperience, currentExperience;
    
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
}

