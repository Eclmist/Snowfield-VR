﻿using System.Collections;
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
            QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>();
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
        storyQuest = QuestManager.Instance.CreateNewStoryLines();
        foreach (QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            StoryQuest newQuest = QuestManager.Instance.GetQuest(line);
            if (newQuest != null)
            {
                QuestEntry<StoryQuest> questEntry = new QuestEntry<StoryQuest>();
                line.Quest = questEntry;
            }
        }
    }

    public bool QuestProgess()
    {
        foreach (QuestEntryGroup<StoryQuest> questEntryGroup in storyQuest)
        {
            if (questEntryGroup.Completed)
                continue;
            else if (!questEntryGroup.Quest.Completed && questEntryGroup.Quest.Started)
            {
                questEntryGroup.Quest.ProgressQuest();
                return true;
            }
        }
        return false;
    }

    public void ResetChecked()
    {
        foreach(QuestEntryGroup<StoryQuest> line in storyQuest)
        {
            line.Quest.Checked = false;
        }
    }

}
