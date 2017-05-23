using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBook {

    public struct QuestEntry {

        Quest currentQuest;
        bool isCompleted;

        public QuestEntry(Quest quest)
        {
            isCompleted = false;
            currentQuest = quest;
        }
    }

    private List<StoryLine> storyQuest = new List<StoryLine>();

    public List<StoryLine> Quests
    {
        get
        {
            return storyQuest;
        }
    }

    //private void RequestNextQuest(Quest hunt)
    //{
    //    StoryLine temp = GetStoryLine(hunt.JobType);
    //    if(temp != null)
    //        StoryQuest newQuest = 
    //}
    
    public StoryQuest GetQuest(bool completed)
    {
        return null;
    }

    public void StartQuest(Quest hunt)
    {

    }

    public void EndQuest(Quest hunt)
    {

    }

    public StoryLine GetStoryLine(JobType jt)
    {
        StoryLine temp = null;

        foreach (StoryLine sl in storyQuest)
        {
            if (jt == sl.JobType)
            {
                temp = sl;
                break;
            }
        }

        return temp;
    }

    public QuestBook()
    {
        storyQuest = QuestManager.Instance.CreateNewStoryLines();
    }

}
