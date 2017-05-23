//using System.Collections.Generic;
//using System;
//using System.Xml;
//using System.Xml.Serialization;

//public enum QuestType
//{
//    DailyQuest,
//    StoryQuest
//}

///* ReadME (Example of obtaining the stanleyReward GameObject reference)
//using UnityEngine;
//using System.IO;

//public class Test : MonoBehaviour {

//    GameObject item;

//    void Start () {
//        QuestManager.QuestList = QuestFactory.Load(Path.Combine(Application.dataPath, "XML/quests.xml"));
//        Debug.Log(QuestManager.QuestList.Count);
//	}

//	void Update () {

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            for(int i = 0; i < QuestManager.QuestList.Count; i++)
//            {
//                if (QuestManager.QuestList[i].index == 2)
//                {
//                    item = Resources.Load("Items/" + QuestManager.QuestList[i].itemType + "/" + QuestManager.QuestList[i].childType + "/" + QuestManager.QuestList[i].stanleyReward) as GameObject;
//                }
//            }
//            Instantiate(item);
//        }
//    }
//}
// */

//[Serializable]
//public class Quest
//{
   

//    [XmlAttribute("index")]
//    public int index;
//    public string title;
//    public QuestType questType;
//    public string stanleyReward;
//    public string[] playerDialog;
//    public int requiredExperience;
//    public JobType questJob;

//    [NonSerialized]
//    public string itemType;
//    [NonSerialized]
//    public string childType;

//    public bool completed;

//    public Quest()
//    {
//        index = 99;
//        title = "default";
//        questType = QuestType.StoryQuest;
//        stanleyReward = null;
//        playerDialog = null;
//        requiredExperience = 0;
//        questJob = JobType.ALCHEMY;

//        completed = true;
//    }

//    public Quest(int index, string title, QuestType questType, string reward, string[] playerDialog, int experience, JobType job, string itemType, string childType)
//    {
//        this.index = index;
//        this.title = title;
//        this.questType = questType;
//        stanleyReward = reward;
//        this.playerDialog = playerDialog;
//        requiredExperience = experience;
//        questJob = job;

//        this.itemType = itemType;
//        this.childType = childType;

//        completed = false;
//    }
//}
