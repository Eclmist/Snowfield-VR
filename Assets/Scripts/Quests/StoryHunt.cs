using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryHunt : Hunt
{
    [SerializeField]private int progressionIndex;
    [SerializeField]private int requiredLevel;
    private JobType storyLine;

    public StoryHunt(JobType jobType):base(jobType)
    {
        
    }

    public StoryHunt(string name, JobType jobType, GameObject reward, Session dialog, int experience, int requiredLevel, int progressionIndex) 
        : base(name,jobType, reward, dialog, experience)
    {

        this.progressionIndex = progressionIndex;
        this.requiredLevel = requiredLevel;
    }

    public int ProgressionIndex
    {
        get { return this.progressionIndex; }
        set { this.progressionIndex = value; }
    }

    public int RequiredLevel
    {
        get { return this.requiredLevel; }
        set { this.requiredLevel = value; }
    }

    public JobType StoryLine
    {
        get { return this.storyLine; }
        set { this.storyLine = value; }
    }
    
}
