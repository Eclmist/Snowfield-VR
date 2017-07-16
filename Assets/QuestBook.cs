using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public void RequestNextQuest(QuestEntryGroup<StoryQuest> group)
    {

        group.ProgressionIndex++;
        StoryQuest newQuest = QuestManager.Instance.GetQuest(group);

        if (newQuest == null)
            group.Completed = true;
        else
        {
            QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>((newQuest.RequiredLevel + 1) * (int)(GameManager.Instance.GameClock.SecondsPerDay / 24f));
            group.Quest = questEntry;
        }

    }

    public QuestEntryGroup<StoryQuest> GetCompletableGroup()
    {
        foreach (QuestEntryGroup<StoryQuest> group in storyQuest)
        {
            if (group.Completed)
                continue;
            else if ((group.Quest.Completed) && !group.Quest.Checked)
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
            if (group.Completed)
                continue;
            else if (!group.Quest.Started && QuestManager.Instance.CanStartQuest(group) && !group.Quest.Checked)
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

    public QuestBook()
    {
        if (QuestManager.Instance)
        {
            storyQuest = QuestManager.Instance.CreateNewStoryLines();
            foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
            {
                line.ProgressionIndex = -1;
                RequestNextQuest(line);
            }
        }

    }

    public QuestEntry<StoryQuest> GetFastestQuest()
    {
        float shortestTime = 999999999;
        QuestEntry<StoryQuest> fastestQuest = null;
        foreach (QuestEntryGroup<StoryQuest> questEntryGroup in storyQuest)
        {
            Debug.Log(questEntryGroup.Quest.Started);
            if (questEntryGroup.Completed)
                continue;
            else if (!questEntryGroup.Quest.Completed && questEntryGroup.Quest.Started && questEntryGroup.Quest.RemainingProgress <= shortestTime)
            {

                shortestTime = questEntryGroup.Quest.RemainingProgress;
                fastestQuest = questEntryGroup.Quest;
            }
        }
        return fastestQuest;
    }

    public void ResetChecked()
    {
        foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            line.Quest.Checked = false;
        }
    }

}
