/* 
 Author : Ivan Leong
 Description : Quest Maker
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class QuestMaker : EditorWindow
{
    public static QuestMaker questMaker;

    List<Quest> questCollection;

    //-----Quests-----//

    int questIndex;

    int experience;

    string questTitle;

    //-----QuestTypes-----//
    TextAsset questTypes;
    string[] questType;

    int questTypeIndex = 0;

    Quest.QuestType questEnum;

    //-----QuestTexts-----//
    string[] playerText = new string[10];

    int textCount = 1;

    //-----jamesRewards-----//
    TextAsset jamesRewards;
    string[] jamesReward;
    int jamesRewardIndex = 0;

    //-----QuestRequiredItems-----//
    TextAsset questItems;
    string[] questItem;
    int questItemIndex = 0;

    //-----Job-----//
    string[] job = new string[2] { "BlackSmith",  "Alchemy"};
    JobType jobIndex;

    JobType jobItemIndex;

    //-----ItemType-----//
    string[] alchemyItems = new string[2] { "Potions", "Dusts" };
    string[] blacksmithItems = new string[2] { "Ores", "Ingots" };
    int itemTypeIndex;

    [MenuItem ("Window/Quest/Quest Maker")]
    protected static void Init()
    {
        questMaker = (QuestMaker)EditorWindow.GetWindow(typeof(QuestMaker));
        questMaker.minSize = new Vector2(800, 400);
        questMaker.Show();
    }

    protected void OnEnable()
    {
        //QuestManager.Save(Path.Combine(Application.dataPath, "Scripts/Quests/quests.xml"));
        ////---------------------------Init Quest-----------------------//
        QuestManager.QuestList = QuestFactory.Load(Path.Combine(Application.dataPath, "Scripts/Quests/quests.xml"));

        questIndex = QuestManager.QuestList.Count;

        Debug.Log(questIndex + " quest(s) loaded");

        //---------------------------Init QuestTypes-----------------------//
        questTypes = Resources.Load("Quests/QuestTypes") as TextAsset;

        if (questTypes == null)
        {
            Debug.LogError("QuestTypes.txt is missing!");
        }
        else
        {
            questType = questTypes.text.Split('\n');
        }

        //---------------------------Init jamesRewards-----------------------//
        jamesRewards = Resources.Load("Quests/questRewards") as TextAsset;

        if (jamesRewards == null)
        {
            Debug.LogError("questReward.txt is missing!");
        }
        else
        {
            jamesReward = jamesRewards.text.Split('\n');
        }

        //---------------------------Init QuestRequiredItems-----------------------//
        questItems = Resources.Load("Quests/QuestRequiredItems") as TextAsset;

        if (questItems == null)
        {
            Debug.LogError("QuestRequiredItems.txt is missing!");
        }
        else
        {
            questItem = questItems.text.Split('\n');
        }
    }
    
    protected void OnGUI()
    {
        GUILayout.Label(("Quest: " + QuestManager.QuestList.Count), EditorStyles.boldLabel);
        
        GUILayout.Label("Quest Title: ", EditorStyles.boldLabel);
        questTitle = EditorGUILayout.TextArea(questTitle, EditorStyles.textArea);

        GUILayout.Label("Quest Type", EditorStyles.boldLabel);
        questTypeIndex = EditorGUILayout.Popup("Quest Types", questTypeIndex, questType);

        GUILayout.Label("Required Experience", EditorStyles.boldLabel);
        jobIndex = (JobType)EditorGUILayout.EnumPopup("Jobs", jobIndex);
        experience = EditorGUILayout.IntField("Required Experience: ", experience);

        GUILayout.Label("James' Rewards", EditorStyles.boldLabel);
        if (questTypeIndex == 0) //If questType == Daily Quest
        {
            jamesRewards = Resources.Load("Quests/questRewards") as TextAsset;

            if (jamesRewards == null)
            {
                Debug.LogError("questReward.txt is missing!");
            }
            else
            {
                jamesReward = jamesRewards.text.Split('\n');
            }

            jamesRewardIndex = EditorGUILayout.Popup("Quest Rewards", jamesRewardIndex, jamesReward);
        }
        else //If questType == Story Quest
        {
            jobItemIndex = (JobType)EditorGUILayout.EnumPopup("Jobs", jobItemIndex);

            if (jobItemIndex == JobType.BLACKSMITH) //If job = Blacksmith
            {
                itemTypeIndex = EditorGUILayout.Popup("Item Types: ", itemTypeIndex, blacksmithItems);

                jamesRewards = Resources.Load("Quests/Items/" + blacksmithItems[itemTypeIndex]) as TextAsset;

                if (jamesRewards == null)
                {
                    Debug.LogError(blacksmithItems[itemTypeIndex] + ".txt is missing!");
                }
                else
                {
                    jamesReward = jamesRewards.text.Split('\n');
                }
            }
            else //If job = Alchemy
            {
                itemTypeIndex = EditorGUILayout.Popup("Item Types: ", itemTypeIndex, alchemyItems);

                jamesRewards = Resources.Load("Quests/Items/" + alchemyItems[itemTypeIndex]) as TextAsset;

                if (jamesRewards == null)
                {
                    Debug.LogError(alchemyItems[itemTypeIndex] + ".txt is missing!");
                }
                else
                {
                    jamesReward = jamesRewards.text.Split('\n');
                }
            }

            jamesRewardIndex = EditorGUILayout.Popup("Quest Rewards", jamesRewardIndex, jamesReward);
        }

        GUILayout.Label("Quest Required Items", EditorStyles.boldLabel);
        questItemIndex = EditorGUILayout.Popup("Quest Required Items", questItemIndex, questItem);

        GUILayout.Label("Quest Dialog", EditorStyles.boldLabel);
        for(int i = 0; i < textCount; i++)
        {
            playerText[i] = EditorGUILayout.TextField("Stanley Dialog", playerText[i]);
        }
        EditorGUILayout.Space();


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if(GUILayout.Button("Create Quest"))
        {
            CreateQuest();
        }

        if(GUILayout.Button("Add Extra Dialog"))
        {
            AddDialog();
        }

        if(GUILayout.Button("Remove Extra Dialog"))
        {
            RemoveDialog();
        }

        if(GUILayout.Button("Clear Quests List"))
        {
            Clear();
        }
    }

    protected void CreateQuest()
    {
        string[] tempDialog = new string[10];

        //----------Write To List----------//

        if (questTypeIndex == 0)
        {
            questEnum = Quest.QuestType.DailyQuest;
        }
        else
        {
            questEnum = Quest.QuestType.StoryQuest;
        }

        Array.Copy(playerText, tempDialog, textCount);

        Quest quest = new Quest(QuestManager.QuestList.Count, questTitle, questEnum, jamesReward[jamesRewardIndex], questItem[questItemIndex], tempDialog, experience, jobIndex);

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
        QuestFactory.Save(Path.Combine(Application.dataPath, "Scripts/Quests/quests.xml"));
        Debug.Log(QuestManager.QuestList.Count + " quest(s) saved");
    }
}
