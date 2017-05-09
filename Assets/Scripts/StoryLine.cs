using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryLine {

    [SerializeField]
    private JobType jobType;
    [SerializeField]
    private List<StoryHunt> storyLine;

    public StoryLine(JobType jobType)
    {
        this.jobType = jobType;
        storyLine = new List<StoryHunt>();
        storyLine.Add(new StoryHunt(jobType));
    }

    public JobType JobType
    {
        get { return this.jobType; }
    }

    public List<StoryHunt> Storyline
    {
        get { return this.storyLine; }
        set { this.storyLine = value; }
    }
        
    public int Count
    {
        get { return storyLine.Count; }
    }

    public void Add(StoryHunt _item)
    {
        storyLine.Add(_item);
    }

    public StoryHunt this[int index]   // Indexer declaration  
    {
        get
        {
            return storyLine[index];
        }
        set
        {
            storyLine[index] = value;
        }
    }
   
   

    public bool Contains(StoryHunt _item)
    {
        return (storyLine.Contains(_item));
    }

    public void Clear()
    {
        storyLine.Clear();
    }


}
