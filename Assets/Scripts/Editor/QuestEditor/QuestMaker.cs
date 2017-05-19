/* 
 Author : Ivan Leong
 Description : Quest Maker
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class QuestMaker : EditorWindow
{
    public static QuestMaker questMaker;

    List<Quest> questCollection;

    //-----Quests-----//
    int experience;

    string questTitle;

    QuestType questEnum;

    //-----QuestTexts-----//
    string[] playerText = new string[10];

    int textCount = 1;

    //-----stanleyRewards-----//
    string[] stanleyReward;
    GameObject[] stanleyRewardGO;
    int stanleyRewardIndex = 0;

    //-----Job-----//
    JobType jobIndex;

    //-----ItemType-----//
    string[] itemTypes;
    int itemTypeIndex;

    string[] childTypes;
    int childTypesIndex;

    [MenuItem ("Editors/Quest/Story Quest Maker")]
    protected static void Init()
    {
        questMaker = (QuestMaker)EditorWindow.GetWindow(typeof(QuestMaker));
        questMaker.minSize = new Vector2(800, 400);
        questMaker.Show();
    }

    protected void OnEnable()
    {
        //---------------------------Init Item Types-----------------------//
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items");
        FileInfo[] fileinfo = directoryInfo.GetFiles();

        itemTypes = new string[fileinfo.Length];

        for (int i = 0; i < fileinfo.Length; i++)
        {
            itemTypes[i] = Path.GetFileNameWithoutExtension(fileinfo[i].FullName);
        }
        //---------------------------Init Quest-----------------------//
        QuestManager.QuestList = QuestFactory.Load(Path.Combine(Application.dataPath, "XML/quests.xml"));

        Debug.Log(QuestManager.QuestList.Count + " quest(s) loaded");
    }

    protected void OnGUI()
    {
        GUILayout.Label(("Quest: " + QuestManager.QuestList.Count), EditorStyles.boldLabel);

        GUILayout.Label("Quest Title: ", EditorStyles.boldLabel);
        questTitle = EditorGUILayout.TextArea(questTitle, EditorStyles.textArea);

        GUILayout.Label("Required Experience", EditorStyles.boldLabel);
        jobIndex = (JobType)EditorGUILayout.EnumPopup("Jobs", jobIndex);
        experience = EditorGUILayout.IntField("Required Experience: ", experience);

        GUILayout.Label("Stanley's Rewards", EditorStyles.boldLabel);

        itemTypeIndex = EditorGUILayout.Popup("Item Types", itemTypeIndex, itemTypes);

        DirectoryInfo childDirectoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items/" + itemTypes[itemTypeIndex]);
        FileInfo[] childFileInfo = childDirectoryInfo.GetFiles();

        childTypes = new string[childFileInfo.Length];

        for (int i = 0; i < childFileInfo.Length; i++)
        {
            childTypes[i] = Path.GetFileNameWithoutExtension(childFileInfo[i].FullName);
        }

        childTypesIndex = EditorGUILayout.Popup("Item Category: ", childTypesIndex, childTypes);

        stanleyRewardGO = Resources.LoadAll("Items/" + itemTypes[itemTypeIndex] + "/" + childTypes[childTypesIndex], typeof(GameObject)).Cast<GameObject>().ToArray();

        stanleyReward = new string[stanleyRewardGO.Length];

        for(int i = 0; i< stanleyRewardGO.Length; i++)
        {
            stanleyReward[i] = stanleyRewardGO[i].name;
        }

        stanleyRewardIndex = EditorGUILayout.Popup("Quest Rewards", stanleyRewardIndex, stanleyReward);

        GUILayout.Label("Quest Dialog", EditorStyles.boldLabel);
        for (int i = 0; i < textCount; i++)
        {
            playerText[i] = EditorGUILayout.TextField("Stanley Dialog", playerText[i]);
        }
        EditorGUILayout.Space();


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Quest"))
        {
            CreateQuest();
        }

        if (GUILayout.Button("Add Extra Dialog"))
        {
            AddDialog();
        }

        if (GUILayout.Button("Remove Extra Dialog"))
        {
            RemoveDialog();
        }

        if (GUILayout.Button("Clear Quests List") && EditorUtility.DisplayDialog("Confirmation", "Once the list is cleared, it cannot be undo. Are you sure?", "Yes I'm sure", "No"))
        {
            Clear();
        }
    }

    protected void CreateQuest()
    {
        string[] tempDialog = new string[10];

        //----------Write To List----------//
        questEnum = QuestType.StoryQuest;

        Array.Copy(playerText, tempDialog, textCount);

        Quest quest = new Quest(QuestManager.QuestList.Count, questTitle, questEnum, stanleyReward[stanleyRewardIndex],tempDialog, experience, jobIndex, itemTypes[itemTypeIndex], childTypes[childTypesIndex]);

        QuestManager.QuestList.Add(quest);

        Debug.Log("Quest (" + (QuestManager.QuestList.Count - 1) +"): " + questTitle +" added");
    }

    protected void AddDialog()
    {
        if(textCount != 10 || textCount < 10)
        {
            questMaker.minSize = new Vector2(questMaker.minSize.x, questMaker.minSize.y + 20);
            textCount++;
        }
    }

    protected void RemoveDialog()
    {
        if(textCount != 1 || textCount > 1)
        {
            questMaker.minSize = new Vector2(questMaker.minSize.x, questMaker.minSize.y - 20);
            textCount--;
        }
    }

    protected void Clear()
    {
        QuestManager.QuestList.Clear();
    }

    protected void OnDisable()
    {
        QuestFactory.Save(Path.Combine(Application.dataPath, "XML/quests.xml"));
        Debug.Log(QuestManager.QuestList.Count + " quest(s) saved");
    }
}
