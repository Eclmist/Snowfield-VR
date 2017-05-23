using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBook
{

    private List<QuestEntryGroup<StoryQuest>> storyQuest = new List<QuestEntryGroup<StoryQuest>>();

    public List<QuestEntryGroup<StoryQuest>> StoryQuests
    {
        get
        {
            return storyQuest;
        }
    }

    private void RequestNextQuest(QuestEntryGroup<StoryQuest> group)
    {
        
            group.ProgressionIndex++;
            StoryQuest newQuest = QuestManager.Instance.GetQuest(group);
            if (newQuest == null)
                group.Completed = true;
            else
                group.Add(newQuest);
        
    }

    public StoryQuest GetCompletableQuest(QuestEntryGroup<StoryQuest> group)
    {
     
        if (group.Quest[group.ProgressionIndex].IsCompleted)
        {
            RequestNextQuest(group);
            return group.Quest[group.ProgressionIndex];
        }

        return null;
    }

    public StoryQuest GetStartableQuest(QuestEntryGroup<StoryQuest> group)
    {
        if (QuestManager.Instance.CanStartQuest(group.Quest[group.ProgressionIndex]))
            return group.Quest[group.ProgressionIndex];

        return null;
    }

    public void StartStoryQuest(StoryQuest hunt)
    {
        Debug.Log("hgrhdgfhdgh");
            //Start Quest routine
    }

    public void EndStoryQuest(StoryQuest hunt)
    {
        //Give xp and item here
    }

    public QuestEntryGroup<StoryQuest> GetStoryGroup(JobType jt)
    {

        foreach (QuestEntryGroup<StoryQuest> sl in storyQuest)
        {
            if (jt == sl.JobType)
            {
                return sl;
            }
        }

        return null;
    }

    public QuestBook()
    {
        storyQuest = QuestManager.Instance.CreateNewStoryLines();
        foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            StoryQuest newQuest = QuestManager.Instance.GetQuest(line);
            if (newQuest != null)
                line.Add(newQuest);
        }
    }



}
