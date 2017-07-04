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

    public void RequestNextQuest(QuestEntry<StoryQuest> quest)
    {

        QuestEntryGroup<StoryQuest> group = GetStoryGroup(quest.Quest.JobType);

        group.ProgressionIndex++;
        StoryQuest newQuest = QuestManager.Instance.GetQuest(group);

        if (newQuest == null)
            group.Completed = true;
        else
        {
            QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>(newQuest);
            group.Add(questEntry);
        }

    }

    public QuestEntry<StoryQuest> GetCompletableQuest(QuestEntryGroup<StoryQuest> group)
    {

        if (group.Completed || group[group.ProgressionIndex].Checked)
            return null;
        else if ((group[group.ProgressionIndex].Completed))
        {
            return group[group.ProgressionIndex];
        }

        return null;
    }

    public QuestEntry<StoryQuest> GetStartableQuest(QuestEntryGroup<StoryQuest> group)
    {
        if (group.Completed)
            return null;
        else if (!group[group.ProgressionIndex].Started && QuestManager.Instance.CanStartQuest(group[group.ProgressionIndex]))
            return group.Quest[group.ProgressionIndex];

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
        storyQuest = QuestManager.Instance.CreateNewStoryLines();
        foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            StoryQuest newQuest = QuestManager.Instance.GetQuest(line);
            if (newQuest != null)
            {
                QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>(newQuest);
                line.Add(questEntry);
            }
        }
    }

    public bool QuestProgess()
    {
        foreach (QuestEntryGroup<StoryQuest> questEntryGroup in storyQuest)
        {
            if (questEntryGroup.Completed)
                continue;
            else if (!questEntryGroup[questEntryGroup.ProgressionIndex].Completed && questEntryGroup[questEntryGroup.ProgressionIndex].Started)
            {
                questEntryGroup[questEntryGroup.ProgressionIndex].ProgressQuest();
                return true;
            }
        }
        return false;
    }

    public void ResetChecked()
    {
        foreach(QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            line[line.ProgressionIndex].Checked = false;
        }
    }

}
