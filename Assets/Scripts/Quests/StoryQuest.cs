using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryQuest : Quest
{
    //[SerializeField]private int progressionIndex;
    [SerializeField]private int requiredLevel;
    private JobType storyLineType;

    public StoryQuest(JobType jobType):base(jobType)
    {
        
    }

    public StoryQuest(string name, JobType jobType, GameObject reward, Session dialog, int experience, int requiredLevel, int progressionIndex) 
        : base(name,jobType, reward, dialog, experience)
    {
        this.requiredLevel = requiredLevel;
    }

    public int RequiredLevel
    {
        get { return this.requiredLevel; }
        set { this.requiredLevel = value; }
    }

    public JobType StoryType
    {
        get { return this.storyLineType; }
        set { this.storyLineType = value; }
    }

    public StoryQuest() { }
    
}
