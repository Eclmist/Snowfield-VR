using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("QuestCollection")]
public class QuestManager
{
    [XmlArray("Quests")]
    [XmlArrayItem("Quest")]
    public static List<Quest> QuestList = new List<Quest>();

    protected bool CheckQuestStates(int questIndex)
    {
        return QuestList[questIndex].completed;
    }
}
