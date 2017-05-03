using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobType
{
    BLACKSMITH,
    ALCHEMY
}


public class Job {

    protected JobType currentJobType;
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
}


//level = constant * squareroot(experience)
//level/constant = squareroot(experience)
//square(level/constant) = experience