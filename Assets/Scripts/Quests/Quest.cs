using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Quest
{
    public enum QuestType
    {
        DailyQuest,
        StoryQuest
    }

    [XmlAttribute("index")]
    public int index;
    public string title;
    public QuestType questType;
    public string jamesReward;
    public string requiredItem;
    public string[] playerDialog;

    public int requiredExperience;
    public JobType questJob;

    public bool completed;

    public Quest()
    {
        index = 99;
        title = "default";
        questType = QuestType.StoryQuest;
        jamesReward = "default";
        requiredItem = "default";
        playerDialog = null;
        requiredExperience = 0;
        questJob = JobType.ALCHEMY;

        completed = true;
    }

    public Quest(int index, string title, QuestType questType, string reward, string requiredItem, string[] playerDialog, int experience, JobType job)
    {
        this.index = index;
        this.title = title;
        this.questType = questType;
        jamesReward = reward;
        this.requiredItem = requiredItem;
        this.playerDialog = playerDialog;
        requiredExperience = experience;
        questJob = job;

        completed = false;
    }
}
