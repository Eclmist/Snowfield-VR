using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class QuestFactory
{

    public static void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, QuestManager.QuestList);
            Debug.Log("Saved Quest(s)");
        }
    }

    public static List<Quest> Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            Debug.Log("Loaded Quest(s)");
            return serializer.Deserialize(stream) as List<Quest>;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static List<Quest> LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>));
        return serializer.Deserialize(new StringReader(text)) as List<Quest>;
    }


    // Returns null if no story quest avaliable
    public static Quest GetLatestAvailableQuest(JobType job)
    {
        /*
         * find all quest for jobtype
         * get rid of locked quests (NOT ENOUGH MASTERY)
         * get rid of all completed
         * if multiple left, return smallest index
         * if 1 left, return that 1
         * if no left, return null
         * */

        List<Quest> availableQuest = new List<Quest>();

        for(int i = 0; i < QuestManager.QuestList.Count; i++)
        {
            if(QuestManager.QuestList[i].questJob == job)
            {
                //if(Player.Instance.getJob(job).Experience >= QuestManager.QuestList[i].requiredExperience && !QuestManager.QuestList[i].completed == false) //TODO: Delete this when getPlayerExperience is implemented
                availableQuest.Add(QuestManager.QuestList[i]);
            }
        }

        if(availableQuest.Count > 0)
        {
            return availableQuest[0];
        }
        else
        {
            return null;
        }
    }

}
