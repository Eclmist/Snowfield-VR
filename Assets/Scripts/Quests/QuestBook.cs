using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBook
{

    private List<QuestEntryGroup<StoryQuest>> storyQuest;

    public List<QuestEntryGroup<StoryQuest>> StoryQuests
    {
        get
        {
            return storyQuest;
        }
    }

    public void RequestNextQuest(QuestEntryGroup<StoryQuest> group)
    {

        int nextIndex = group.ProgressionIndex + 1;
        StoryQuest newQuest = QuestManager.Instance.GetQuest(nextIndex, group.JobType);

        if (newQuest != null)
            group.ProgressionIndex = nextIndex;
        else
            newQuest = QuestManager.Instance.GetQuest(group.ProgressionIndex, group.JobType);

        QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>((newQuest.RequiredLevel + 1) * (int)(GameManager.Instance.GameClock.SecondsPerDay / 24f));
        group.Quest = questEntry;

    }

    public QuestEntryGroup<StoryQuest> GetCompletableGroup()
    {
        foreach (QuestEntryGroup<StoryQuest> group in storyQuest)
        {
            if (group.Quest.Completed)
            {
                return group;
            }
        }

        return null;
    }


    public QuestEntryGroup<StoryQuest> GetStartableGroup()
    {
        foreach (QuestEntryGroup<StoryQuest> group in storyQuest)
        {
            if (!group.Quest.Started && QuestManager.Instance.CanStartQuest(group))
                return group;
        }

        return null;
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

    public void BeginQuestBook()
    {
        storyQuest = QuestManager.Instance.CreateNewStoryLines();
        foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            line.ProgressionIndex = -1;
            RequestNextQuest(line);
        }
    }

    public QuestEntry<StoryQuest> GetFastestQuest()
    {
        float shortestTime = 999999999;
        QuestEntry<StoryQuest> fastestQuest = null;
        foreach (QuestEntryGroup<StoryQuest> questEntryGroup in storyQuest)
        {
            if (!questEntryGroup.Quest.Completed && questEntryGroup.Quest.Started && questEntryGroup.Quest.RemainingProgress <= shortestTime)
            {
                shortestTime = questEntryGroup.Quest.RemainingProgress;
                fastestQuest = questEntryGroup.Quest;
            }
        }
        return fastestQuest;
    }


}
