using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public enum QuestType
    {
        DailyQuest,
        StoryQuest
    }

    public static List<Quest> QuestList = new List<Quest>();

    public int index;
    public string title;
    public QuestType questType;
    public string jamesReward;
    public string requiredItem;
    public string[] playerDialog;

    public int requiredExperience;
    public string questJob;

    private bool completed;

    public Quest(int index, string title, QuestType questType, string reward, string requiredItem, string[] playerDialog, int experience, string job)
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



    // Returns null if no story quest avaliable
    //public static Quest GetLatestAvaliableStoryQuest(Job)
    //{
        

    //    /*
    //     * find all quest for jobtype
    //     * get rid of locked quests (NOT ENOUGH MASTERY)
    //     * get rid of all completed
    //     * if multiple left, return smallest index
    //     * if 1 left, return that 1
    //     * if no left, return null
    //     * */

    //    return null;
    //}
}
