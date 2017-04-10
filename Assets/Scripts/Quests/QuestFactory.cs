using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class QuestFactory : MonoBehaviour {

    public static void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>));
        using (FileStream stream = new FileStream(path, FileMode.Append))
        {
            serializer.Serialize(stream, QuestManager.QuestList);
            stream.Close();
        }
    }

    public static List<Quest> Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
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
