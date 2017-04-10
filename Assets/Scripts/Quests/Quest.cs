using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Quest
{
    [XmlArray("Quests"), XmlArrayItem("Quest")]
    public static List<Quest> QuestList = new List<Quest>();

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
    public string questJob;

    private bool completed;

    public Quest()
    {
        index = 99;
        title = "default";
        questType = QuestType.StoryQuest;
        jamesReward = "default";
        requiredItem = "default";
        playerDialog = null;
        requiredExperience = 0;
        questJob = "default";

        completed = true;
    }

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

    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Quest));
        using (FileStream stream = new FileStream(path, FileMode.Append))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static Quest Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Quest));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Quest;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Quest LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Quest));
        return serializer.Deserialize(new StringReader(text)) as Quest;
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
